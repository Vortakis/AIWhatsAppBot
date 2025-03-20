using System.Data;
using AIWAB.Common.Configuration.ExternalAI;
using ExtAIProviderAPI.AIProviders;
using ExtAIProviderAPI.Models;
using ExtAIProviderAPI.Models.Enum;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OpenAI;
using OpenAI.Chat;

namespace ExtAIProviderAPI.AIProviders.OpenAI;

public class OpenAIProvider : IAIProvider
{
    private readonly OpenAIClient _openAIClient;
    private readonly Dictionary<string, AIUsageSettings> _openAiUsageSettings;

    public OpenAIProvider(OpenAIClient openAIClient, IOptions<ExternalAISettings> externalAISettings)
    {
        _openAIClient = openAIClient;
        _openAiUsageSettings = externalAISettings.Value.AIUsage;
    }

    public async Task<AIResponse> ProcessAsync(string input, AIPromptType promptType)
    {
        switch (promptType)
        {
            case AIPromptType.QnA:
                return await ProcessQnAAsync(input);
            default:
                throw null;
        }


    }

    private async Task<AIResponse> ProcessQnAAsync(string input)
    {
        List<ChatMessage> chatMessages = new List<ChatMessage>
        {
            new SystemChatMessage("You are a helpful assistant answering eToro-related questions."),
            new UserChatMessage(input)
        };

        ChatCompletionOptions options = new ChatCompletionOptions
        {
            MaxOutputTokenCount = _openAiUsageSettings["QnA"].MaxTokens,
            Temperature = _openAiUsageSettings["QnA"].Temperature,

        };

        var response = await _openAIClient.GetChatClient(_openAiUsageSettings["QnA"].Model).CompleteChatAsync(chatMessages);
        return new AIResponse { Answer = response.Value.Content[0].Text.Trim() };
    }
}
