
using AIProviderAPI.AIProviders;
using AIProviderAPI.Models.DTOs;
using AIWAB.Common.Configuration.ExternalAI;
using AIWAB.Common.Core.AIProviderAPI.Enum;
using Microsoft.Extensions.Options;

namespace AIProviderAPI.Services
{
    public class AIProviderService : IAIProviderService
    {
        private readonly IAIProvider _aiProvider;
        private readonly bool _enabled;
        private readonly ILogger<OpenAIProvider> _logger;

        public AIProviderService(AIProviderFactory factory, IOptions<ExternalAISettings> externalAISettings, ILogger<OpenAIProvider> logger)
        {
            _aiProvider = factory.GetProvider();
            _enabled = externalAISettings.Value.Enabled;
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
                    return await _aiProvider.ProcessQnAAsync(promptRequest.Prompt);
                case AIPromptType.Embeddings:
                    return await _aiProvider.GetEmbeddingsAsync(promptRequest.Prompt);
                default:
                    throw null;
            }
        }
    }
}
