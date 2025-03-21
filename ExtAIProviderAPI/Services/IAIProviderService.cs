using ExtAIProviderAPI.AIProviders;
using ExtAIProviderAPI.Models.DTOs;

namespace ExtAIProviderAPI.Services;

public interface IAIProviderService
{
    Task<AIResponseDTO> ProcessPromptAsync(AIRequestDTO promptRequest);
}
