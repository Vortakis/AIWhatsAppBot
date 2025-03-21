
using AIProviderAPI.AIProviders;
using AIProviderAPI.Models.DTOs;

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
            //await _aiProvider.ProcessAsync(promptRequest.Prompt, promptRequest.PromptType);

            return await Task.FromResult(new AIResponseDTO { Answer = "AI Disabled manually - hi from me for now :)" });
        }
    }
}
