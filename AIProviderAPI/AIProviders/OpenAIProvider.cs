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
    private readonly MessagingPlatformSettings _msgPlatformSettings;
    private readonly ILogger<OpenAIProvider> _logger;

    public OpenAIProvider(
        OpenAIClient openAIClient,
        IOptions<ExternalAISettings> externalAISettings,
        IOptions<ExternalMsgPlatformSettings> externalMsgSettings,
        ILogger<OpenAIProvider> logger)
    {
        _openAIClient = openAIClient;
        _aiUsageSettings = externalAISettings.Value.AIUsage;
        _msgPlatformSettings = externalMsgSettings.Value.MessagingPlatforms[externalMsgSettings.Value.DefaultPlatform];
        _logger = logger;
    }

    public async Task<AIResponseDTO> ProcessQnAAsync(string input)
    {
        string promptType = AIPromptType.QnA.ToString();
        string searchReferences = string.Join(", ", _aiUsageSettings[promptType].References);
        List<ChatMessage> chatMessages = new List<ChatMessage>
        {
            new SystemChatMessage($"You are a friendly assistant answering only questions related to eToro. " +
            $"Your knowledge comes from these websites: '{searchReferences}'. " +
            $"Provide accurate, concise responses without text formatting (no bold, italics, or markdown). " +
            $"Keep responses under {_msgPlatformSettings.MaxMessageLength} characters."),
            new UserChatMessage(input)
        };

        ChatCompletionOptions options = new ChatCompletionOptions
        {
            MaxOutputTokenCount = _aiUsageSettings[promptType].MaxTokens,
            Temperature = _aiUsageSettings[promptType].Temperature,
        };

        var response = await _openAIClient.GetChatClient(_aiUsageSettings[promptType].Model).CompleteChatAsync(chatMessages);
        return new AIResponseDTO { Answer = response.Value.Content[0].Text.Trim() };
    }

    public async Task<AIResponseDTO> GetEmbeddingsAsync(string input)
    {
        string promptType = AIPromptType.Embeddings.ToString();

        var embeddingClient = _openAIClient.GetEmbeddingClient(_aiUsageSettings[promptType].Model);
        var response = await embeddingClient.GenerateEmbeddingsAsync(new List<string> { input });

        return new AIResponseDTO { Embeddings = response.Value[0].ToFloats().ToArray() };
    }
}
