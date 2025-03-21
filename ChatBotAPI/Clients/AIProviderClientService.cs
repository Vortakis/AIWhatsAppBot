using ExtAIProviderAPI.Protos;

namespace ChatBotAPI.Clients
{
    public class AIProviderClientService : IAIProviderClientService
    {
        public AIProviderService.AIProviderServiceClient _aiProviderClient;

        public AIProviderClientService(AIProviderService.AIProviderServiceClient aiProviderClient)
        {
            _aiProviderClient = aiProviderClient;
        }

        public async Task<AIResponse> PromptAIAsync(AIRequest request)
        {
            return await _aiProviderClient.PromptAIAsync(request);
        }
    }
}
