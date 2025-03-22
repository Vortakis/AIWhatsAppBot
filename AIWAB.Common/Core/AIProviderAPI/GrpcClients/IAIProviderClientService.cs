using AIProviderAPI.Protos;

namespace AIWAB.Common.Core.AIProviderAPI.GrpcClients
{
    public interface IAIProviderClientService
    {
        Task<AIResponse> PromptAIAsync(AIRequest request);
    }
}
