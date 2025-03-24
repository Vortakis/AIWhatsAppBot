using AIWAB.Common.Configuration.ExternalAI;
using AIProviderAPI.Models.DTOs;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Embeddings;
using AIWAB.Common.Core.AIProviderAPI.Enum;
using AIWAB.Common.Configuration.ExternalMsgPlatform;
using AIProviderAPI.Controllers;

namespace AIProviderAPI.AIProviders;

public class OpenAIProvider : IAIProvider
{
    private readonly OpenAIClient _openAIClient;
    private readonly Dictionary<string, AIUsageSettings> _aiUsageSettings;
    private readonly ILogger<OpenAIProvider> _logger;

    public OpenAIProvider(
        OpenAIClient openAIClient,
        IOptions<ExternalAISettings> externalAISettings,
        IOptions<ExternalMsgPlatformSettings> externalMsgSettings,
        ILogger<OpenAIProvider> logger)
    {
        _openAIClient = openAIClient;
        _aiUsageSettings = externalAISettings.Value.AIUsage;
        _logger = logger;
    }

    public async Task<AIResponseDTO> ProcessQnAAsync(string input)
    {
        string promptType = AIPromptType.QnA.ToString();
        string searchReferences = string.Join(", ", _aiUsageSettings[promptType].References);
        List<ChatMessage> chatMessages = new List<ChatMessage>
        {
            new SystemChatMessage($"You are a friendly assistant, answering questions only related with eToro."),
            new SystemChatMessage($"Only use information from these sources: {searchReferences}. Do not generate answers from other knowledge."),
            new SystemChatMessage("Always provide concise, self-contained responses."),
            new SystemChatMessage($"Aim the response to be **as answered and complete as possible** within { _aiUsageSettings[promptType].MaxTokens} tokens."),
            new SystemChatMessage("Strictly do not use text formatting (no bold, italics, or markdown)."),
            new UserChatMessage(input)
        };

        ChatCompletionOptions options = new ChatCompletionOptions
        {
            MaxOutputTokenCount = _aiUsageSettings[promptType].MaxTokens,
            Temperature = _aiUsageSettings[promptType].Temperature,
        };

        bool continuationFlag = false;
        string responseMsg = string.Empty;
        do
        {
            continuationFlag = false;
            var response = await _openAIClient.GetChatClient(_aiUsageSettings[promptType].Model).CompleteChatAsync(chatMessages, options);
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
