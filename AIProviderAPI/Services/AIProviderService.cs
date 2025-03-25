
using AIProviderAPI.AIProviders;
using AIProviderAPI.Models.DTOs;
using AIWAB.Common.Configuration.ExternalAI;
using AIWAB.Common.Configuration.ExternalMsgPlatform;
using AIWAB.Common.Core.AIProviderAPI.Enum;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace AIProviderAPI.Services
{
    public class AIProviderService : IAIProviderService
    {
        private readonly IAIProvider _aiProvider;
        private readonly Dictionary<string, AIUsageSettings> _aiUsageSettings;
        private readonly bool _enabled;
        private readonly ILogger<OpenAIProvider> _logger;

        public AIProviderService(
            AIProviderFactory factory, 
            IOptions<ExternalAISettings> externalAISettings,
            IOptions<ExternalMsgPlatformSettings> externalMsgSettings,
            ILogger<OpenAIProvider> logger)
        {
            _aiProvider = factory.GetProvider();
            _enabled = externalAISettings.Value.Enabled;
            _aiUsageSettings = externalAISettings.Value.AIUsage;
            _logger = logger;
        }

        public async Task<AIResponseDTO> ProcessPromptAsync(AIRequestDTO promptRequest)
        {
            if (!_enabled)
            {
                return await Task.FromResult(new AIResponseDTO { Answer = "AI Disabled manually - hi from me for now :)" });
            }

            switch (promptRequest.PromptType)
            {
                case AIPromptType.QnA:
                case AIPromptType.BulkQnA:
                    string promptType = promptRequest.PromptType.ToString();
                    if (!_aiUsageSettings.TryGetValue(promptType, out var usageSettings))
                    {
                        _logger.LogWarning("AI Usage settings not found for {PromptType}", promptType);
                        throw new InvalidOperationException("AI usage settings missing for QnA.");
                    }

                    List<string> systemInput = new List<string>
                    {
                        "You are a professional and friendly assistant specializing in answering questions only about eToro. " +
                        "Always provide accurate, helpful, and positive information about eToro. " +
                        "If a user asks about a negative topic, respond neutrally and professionally while maintaining a constructive tone. " +
                        "If a question is unrelated to eToro, politely redirect the conversation back to eToro-related topics.",
                        $"Use only information from these sources: {string.Join(", ", _aiUsageSettings[AIPromptType.QnA.ToString()].References)}. Do not generate answers from other knowledge.",
                        $"Aim the response to be **as answered and complete as possible** within {_aiUsageSettings[promptType].MaxTokens} tokens.",
                        "Strictly do not use text formatting (no bold, italics, or markdown)."
                    };

                    return await _aiProvider.ProcessQnAAsync(systemInput, promptRequest.Prompt, promptRequest.PromptType);
                case AIPromptType.Embeddings:
                    return await _aiProvider.GetEmbeddingsAsync(promptRequest.Prompt);
                default:
                    throw new InvalidOperationException("AIPromptType is not recognised.");
            }
        }
    }
}
