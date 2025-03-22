using AIWAB.Common.Core.AIProviderAPI.Enum;
using AIProviderAPI.Models.DTOs;

namespace AIProviderAPI.AIProviders;

public interface IAIProvider
{
    Task<AIResponseDTO> ProcessQnAAsync(string input);

    Task<AIResponseDTO> GetEmbeddingsAsync(string input);
}
