using AIWAB.Common.Configuration.ExternalAI;
using AIProviderAPI.Models.DTOs;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Embeddings;
using AIWAB.Common.Core.AIProviderAPI.Enum;
using AIWAB.Common.Configuration.ExternalMsgPlatform;

namespace AIProviderAPI.AIProviders;

public class OpenAIProvider : IAIProvider
{
    private readonly OpenAIClient _openAIClient;
    private readonly Dictionary<string, AIUsageSettings> _aiUsageSettings;
    private readonly MessagingPlatformSettings _msgPlatformSettings;

    public OpenAIProvider(
        OpenAIClient openAIClient,
        IOptions<ExternalAISettings> externalAISettings,
        IOptions<ExternalMsgPlatformSettings> externalMsgSettings)
    {
        _openAIClient = openAIClient;
        _aiUsageSettings = externalAISettings.Value.AIUsage;
        _msgPlatformSettings = externalMsgSettings.Value.MessagingPlatforms[externalMsgSettings.Value.DefaultPlatform];
    }

    public async Task<AIResponseDTO> ProcessQnAAsync(string input)
    {
        string promptType = AIPromptType.QnA.ToString();
        string searchReferences = string.Join(", ", _aiUsageSettings[promptType].References);
        List<ChatMessage> chatMessages = new List<ChatMessage>
        {
            new SystemChatMessage("You are a helpful assistant answering eToro-related questions."),
            new SystemChatMessage($"You master knowledgebase is from these websites: '{searchReferences}'."),
            new SystemChatMessage("Always provide accurate and concise responses."),
            new UserChatMessage(input)
        };

        if (_msgPlatformSettings.MaxMessageLength > 0)
        {
            var textCharLimit =
                 new SystemChatMessage($"Your concatenated response characters should be less than {_msgPlatformSettings.MaxMessageLength}");

            chatMessages.Add(textCharLimit);
        }


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
