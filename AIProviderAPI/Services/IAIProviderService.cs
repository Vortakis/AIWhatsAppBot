using AIProviderAPI.AIProviders;
using AIProviderAPI.Models.DTOs;

namespace AIProviderAPI.Services;

public interface IAIProviderService
{
    Task<AIResponseDTO> ProcessPromptAsync(AIRequestDTO promptRequest);

}
