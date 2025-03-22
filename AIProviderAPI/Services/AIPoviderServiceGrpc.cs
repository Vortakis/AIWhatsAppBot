using AIWAB.Common.Core.AIProviderAPI.Enum;
using AIProviderAPI.Models.DTOs;
using AIProviderAPI.Protos;
using Grpc.Core;

namespace AIProviderAPI.Services;

public class AIPoviderServiceGrpc : AIProviderAPI.Protos.AIProviderService.AIProviderServiceBase
{
    private readonly IAIProviderService _aiProviderService;

    public AIPoviderServiceGrpc(IAIProviderService aiProviderService)
    {
        _aiProviderService = aiProviderService;
    }

    public override async Task<AIResponse> PromptAI(AIRequest request, ServerCallContext context)
    {
        var aiRequestDTO = new AIRequestDTO
        {
            Prompt = request.Prompt,
            PromptType = Enum.Parse<AIPromptType>(request.PromptType)
        };

        var result = await _aiProviderService.ProcessPromptAsync(aiRequestDTO);
        var aiResponse = new AIResponse
        {
            Answer = result.Answer
        };
        aiResponse.Embeddings.AddRange(result.Embeddings);
        return aiResponse;
    }
}
