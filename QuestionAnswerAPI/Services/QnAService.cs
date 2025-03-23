using AIProviderAPI.Protos;
using AIWAB.Common.Configuration.ExternalAI;
using AIWAB.Common.Core.AIProviderAPI.Enum;
using AIWAB.Common.Core.AIProviderAPI.GrpcClients;
using Microsoft.Extensions.Options;
using QuestionAnswerAPI.Models;
using QuestionAnswerAPI.Repository;

namespace QuestionAnswerAPI.Services;

public class QnAService : IQnAService
{
    private readonly IQnARepository _qnaRepository;
    private readonly IAIProviderClientService _aiProviderClientService;

    private readonly Dictionary<string, AIUsageSettings> _aiUsageSettings;

    private readonly ILogger<QnAService> _logger;

    public QnAService(IQnARepository qnaRepository,
        IAIProviderClientService aiProviderClientService,
        IOptions<ExternalAISettings> externalAISettings, 
        ILogger<QnAService> logger)
    {
        _qnaRepository = qnaRepository;
        _aiProviderClientService = aiProviderClientService;
        _aiUsageSettings = externalAISettings.Value.AIUsage;
        _logger = logger;
    }

    public async Task<QnAModel> GetQnAAsync(string question)
    {
        var foundQnA = _qnaRepository.GetQnA(question);
        if (foundQnA != null)
            return foundQnA;

        var embeddingResult = await _aiProviderClientService.PromptAIAsync(
            new AIRequest
            {
                Prompt = question,
                PromptType = AIPromptType.Embeddings.ToString()
            });

        foundQnA = EmbeddingHelper.GetByEmbedding(embeddingResult.Embeddings.ToArray(), _qnaRepository.GetAllQnA());
        if (foundQnA != null)
            return foundQnA;

        return new QnAModel { Embeddings = embeddingResult.Embeddings.ToArray(), Question = question };
    }

    public async Task AddQnAAsync(QnAModel qnaModel)
    {
        if (qnaModel.Embeddings.Length == 0)
        {
            var embeddingResult = await _aiProviderClientService.PromptAIAsync(
                new AIRequest
                {
                    Prompt = qnaModel.Question,
                    PromptType = AIPromptType.Embeddings.ToString()
                });
            qnaModel.Embeddings = embeddingResult.Embeddings.ToArray();
        }
    
        var newQnA = new QnAModel 
        { 
            Embeddings = qnaModel.Embeddings, 
            Question = qnaModel.Question, 
            Answer = qnaModel.Answer };

        await _qnaRepository.AddQnAAsync(newQnA);
    }
}
