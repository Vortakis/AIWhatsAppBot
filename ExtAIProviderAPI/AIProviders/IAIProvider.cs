using ExtAIProviderAPI.Models.DTOs;
using ExtAIProviderAPI.Models.Enum;

namespace ExtAIProviderAPI.AIProviders;

public interface IAIProvider
{
    Task<AIResponseDTO> ProcessAsync(string input, AIPromptType promptType);
}
