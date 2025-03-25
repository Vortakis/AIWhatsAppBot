using AIWAB.Common.Configuration.ExternalAI;
using AIProviderAPI.Models.DTOs;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;
using AIWAB.Common.Core.AIProviderAPI.Enum;
using AIWAB.Common.Configuration.ExternalMsgPlatform;
using AIProviderAPI.Controllers;

namespace AIProviderAPI.AIProviders;

public class OpenAIProvider : IAIProvider
{
    private readonly OpenAIClient _openAIClient;
    private readonly Dictionary<string, AIUsageSettings> _aiUsageSettings;
    private readonly ILogger<OpenAIProvider> _logger;

    private readonly string _bulkQnASchema;
    private const string _bulkQnASchemaName = "bulkqna_schema";

    public OpenAIProvider(
        OpenAIClient openAIClient,
        IOptions<ExternalAISettings> externalAISettings,
        IOptions<ExternalMsgPlatformSettings> externalMsgSettings,
        ILogger<OpenAIProvider> logger)
    {
        _openAIClient = openAIClient;
        _aiUsageSettings = externalAISettings.Value.AIUsage;
        _logger = logger;

        string schemaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Schemas/{_bulkQnASchemaName}.json");
        _bulkQnASchema = File.ReadAllText(schemaPath);
    }

    public async Task<AIResponseDTO> ProcessQnAAsync(List<string> systemInput, string userInput, AIPromptType promptType)
    {

        List<ChatMessage> chatMessages = new ();

        chatMessages.AddRange(systemInput.Select(s => new SystemChatMessage(s)).ToList());
        chatMessages.Add(new UserChatMessage(userInput));

        ChatCompletionOptions options = new ChatCompletionOptions
        {
            MaxOutputTokenCount = _aiUsageSettings[promptType.ToString()].MaxTokens,
            Temperature = _aiUsageSettings[AIPromptType.QnA.ToString()].Temperature,
        };

        if (promptType == AIPromptType.BulkQnA)
        {

            BinaryData schemaBinaryData = BinaryData.FromString(_bulkQnASchema);
            ChatResponseFormat responseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                jsonSchemaFormatName: _bulkQnASchemaName,
                jsonSchema: schemaBinaryData
            );
            options.ResponseFormat = responseFormat;
        }

        bool continuationFlag = false;
        string responseMsg = string.Empty;
        do
        {
            continuationFlag = false;
            var response = await _openAIClient.GetChatClient(_aiUsageSettings[promptType.ToString()].Model).CompleteChatAsync(chatMessages, options);
            responseMsg = string.Concat(responseMsg, response.Value.Content[0].Text.Trim());

            if (response.Value.FinishReason == ChatFinishReason.Length)
            {
                chatMessages.Add(new AssistantChatMessage(response.Value.Content[0].Text.Trim()));
                chatMessages.Add(new UserChatMessage("Continue from your last point."));
                continuationFlag = true;
            }           

        } while (continuationFlag);

        return new AIResponseDTO { Answer = responseMsg };
    }

    public async Task<AIResponseDTO> GetEmbeddingsAsync(string input)
    {
        string promptType = AIPromptType.Embeddings.ToString();

        var embeddingClient = _openAIClient.GetEmbeddingClient(_aiUsageSettings[promptType].Model);
        var response = await embeddingClient.GenerateEmbeddingsAsync(new List<string> { input });

        return new AIResponseDTO { Embeddings = response.Value[0].ToFloats().ToArray() };
    }
   
}
