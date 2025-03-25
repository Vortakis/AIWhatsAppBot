using AIWAB.Common.Core.AIProviderAPI.Enum;
using AIProviderAPI.Models.DTOs;

namespace AIProviderAPI.AIProviders;

public interface IAIProvider
{
    Task<AIResponseDTO> ProcessQnAAsync(List<string> systemInput, string userInput, AIPromptType promptType);

    Task<AIResponseDTO> GetEmbeddingsAsync(string input);
}
