
using AIProviderAPI.AIProviders;
using AIProviderAPI.Models.DTOs;
using AIWAB.Common.Core.AIProviderAPI.Enum;

namespace AIProviderAPI.Services
{
    public class AIProviderService : IAIProviderService
    {
        private readonly IAIProvider _aiProvider;

        public AIProviderService(AIProviderFactory factory)
        {
            _aiProvider = factory.GetProvider();
        }

        public async Task<AIResponseDTO> ProcessPromptAsync(AIRequestDTO promptRequest)
        {
            switch (promptRequest.PromptType)
            {
                case AIPromptType.QnA:
                    return await _aiProvider.ProcessQnAAsync(promptRequest.Prompt);
                case AIPromptType.Embeddings:
                    return await _aiProvider.GetEmbeddingsAsync(promptRequest.Prompt);
                default:
                    throw null;
            }

          //  return await Task.FromResult(new AIResponseDTO { Answer = "AI Disabled manually - hi from me for now :)" });
        }
    }
}
