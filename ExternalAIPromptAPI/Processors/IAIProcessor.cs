using ExternalAIPromptAPI.Models;

namespace ExternalAIPromptAPI.Processors;

public interface IAIProcessor
{
    Task<AIResponse> ProcessAsync(string input, string taskType);
}
