using ExtAIProviderAPI.AIProviders;
using ExtAIProviderAPI.Models;

namespace ExtAIProviderAPI.Services;

public interface IAIProviderService
{
    Task<AIResponse> ProcessPromptAsync(AIRequest promptRequest);
}
