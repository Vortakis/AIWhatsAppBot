using ExtAIProviderAPI.Models;
using ExtAIProviderAPI.Models.Enum;

namespace ExtAIProviderAPI.AIProviders;

public interface IAIProvider
{
    Task<AIResponse> ProcessAsync(string input, AIPromptType promptType);
}
