using ExtAIProviderAPI.Models.DTOs;
using ExtAIProviderAPI.Models.Enum;
using ExtAIProviderAPI.Protos;
using Grpc.Core;

namespace ExtAIProviderAPI.Services;

public class AIPoviderServiceGrpc : ExtAIProviderAPI.Protos.AIProviderService.AIProviderServiceBase
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

       // var result = await _aiProviderService.ProcessPromptAsync(aiRequestDTO);
        var aiResponse = new AIResponse
        {
            Answer = "It worked!"// result.Answer
        };
        return aiResponse;
    }
}
