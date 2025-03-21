using AIWAB.Common.Core.AIProviderAPI.Enum;
using AIProviderAPI.Models.DTOs;

namespace AIProviderAPI.AIProviders;

public interface IAIProvider
{
    Task<AIResponseDTO> ProcessAsync(string input, AIPromptType promptType);
}
