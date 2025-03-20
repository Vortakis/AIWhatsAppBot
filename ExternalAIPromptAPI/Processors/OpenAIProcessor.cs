using ExternalAIPromptAPI.Models;
using OpenAI;

namespace ExternalAIPromptAPI.Processors;

public class OpenAIProcessor : IAIProcessor
{
    private readonly OpenAIClient _openAIClient;
    private const string _model = "gpt-3.5-turbo";

    public OpenAIProcessor(OpenAIClient openAIClient)
    {
        _openAIClient = openAIClient;
    }

    public Task<AIResponse> ProcessAsync(string input, string taskType)
    {
        throw new NotImplementedException();
    }
}
