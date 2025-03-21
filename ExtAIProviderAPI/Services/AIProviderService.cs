
using ExtAIProviderAPI.AIProviders;
using ExtAIProviderAPI.Models.DTOs;
using ExtAIProviderAPI.Models.Enum;

namespace ExtAIProviderAPI.Services
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
            return await _aiProvider.ProcessAsync(promptRequest.Prompt, promptRequest.PromptType);
        }
    }
}
