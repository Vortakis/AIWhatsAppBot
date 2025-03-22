using AIProviderAPI.Protos;

namespace ChatBotAPI.GrpcClients
{
    public interface IAIProviderClientService
    {
        Task<AIResponse> PromptAIAsync(AIRequest request);
    }
}
