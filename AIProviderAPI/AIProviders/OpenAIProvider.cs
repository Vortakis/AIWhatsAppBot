using AIWAB.Common.Configuration.ExternalAI;
using AIProviderAPI.Models.DTOs;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Embeddings;
using AIWAB.Common.Core.AIProviderAPI.Enum;

namespace AIProviderAPI.AIProviders;

public class OpenAIProvider : IAIProvider
{
    private readonly OpenAIClient _openAIClient;
    private readonly Dictionary<string, AIUsageSettings> _aiUsageSettings;

    public OpenAIProvider(OpenAIClient openAIClient, IOptions<ExternalAISettings> externalAISettings)
    {
        _openAIClient = openAIClient;
        _aiUsageSettings = externalAISettings.Value.AIUsage;
    }

    public async Task<AIResponseDTO> ProcessQnAAsync(string input)
    {
        string promptType = AIPromptType.QnA.ToString();
        List<ChatMessage> chatMessages = new List<ChatMessage>
        {
            new SystemChatMessage("You are a helpful assistant answering eToro-related questions."),
            new UserChatMessage(input)
        };

        ChatCompletionOptions options = new ChatCompletionOptions
        {
            MaxOutputTokenCount = _aiUsageSettings[promptType].MaxTokens,
            Temperature = _aiUsageSettings[promptType].Temperature,

        };

        var chatClient = _openAIClient.GetChatClient(_aiUsageSettings[promptType].Model);
        var response = await chatClient.CompleteChatAsync(chatMessages);
        return new AIResponseDTO { Answer = response.Value.Content[0].Text.Trim() };
    }

    public async Task<AIResponseDTO> GetEmbeddingsAsync(string input)
    {
        string promptType = AIPromptType.Embeddings.ToString();

        var embeddingClient = _openAIClient.GetEmbeddingClient(_aiUsageSettings[promptType].Model);
        var response = await embeddingClient.GenerateEmbeddingsAsync(new List<string> { input });

        return new AIResponseDTO { Embeddings = response.Value[0].ToFloats().ToArray()};
    }
}
