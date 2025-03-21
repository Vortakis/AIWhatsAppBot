using AIProviderAPI.Protos;

namespace ChatBotAPI.Clients
{
    public interface IAIProviderClientService
    {
        Task<AIResponse> PromptAIAsync(AIRequest request);
    }
}
