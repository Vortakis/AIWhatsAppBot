using System.Xml.Linq;
using AIProviderAPI.Protos;
using AIWAB.Common.Configuration.ExternalAI;
using AIWAB.Common.Core.AIProviderAPI.Enum;
using AIWAB.Common.Core.AIProviderAPI.GrpcClients;
using AIWAB.Common.Core.QuestionAnswerAPI.DTOs;
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

    public async Task<QnAModel> GetAnswer(string question)
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

        return new QnAModel { Embedding = embeddingResult.Embeddings.ToArray(), Question = question };
    }

    public async Task AddQnAAsync(QnACreateDTO qnaCreateDTO)
    {
        if (qnaCreateDTO.Embedding.Length == 0)
        {
            var embeddingResult = await _aiProviderClientService.PromptAIAsync(
                new AIRequest
                {
                    Prompt = qnaCreateDTO.Question,
                    PromptType = AIPromptType.Embeddings.ToString()
                });
            qnaCreateDTO.Embedding = embeddingResult.Embeddings.ToArray();
        }
    
        var newQnA = new QnAModel 
        { 
            Embedding = qnaCreateDTO.Embedding, 
            Question = qnaCreateDTO.Question, 
            Answer = qnaCreateDTO.Answer };

        await _qnaRepository.AddQnAAsync(newQnA);
    }
}
