using QuestionAnswerAPI.Models;
using QuestionAnswerAPI.Protos;

namespace QuestionAnswerAPI.Services;

public class QnAServiceGrpc : QuestionAnswerAPI.Protos.QnAService.QnAServiceBase
{
    private readonly IQnAService _qnaService;

    public QnAServiceGrpc(IQnAService qnaService)
    {
        _qnaService = qnaService;
    }

    public async Task<QnADTO> GetQnA(QuestionDTO questionDTO)
    {
        var result = await _qnaService.GetQnAAsync(questionDTO.Question);

        var qnaDTO = new QnADTO
        {
            Question = result.Question,
            Answer = result.Answer
        };

        qnaDTO.Embeddings.AddRange(result.Embeddings);
        return qnaDTO;
    }

    public async Task AddQnA(QnADTO qnaDTO)
    {
        var qnaModel = new QnAModel
        {
            Embeddings = qnaDTO.Embeddings.ToArray(),
            Question = qnaDTO.Question,
            Answer = qnaDTO.Answer
        };
        await _qnaService.AddQnAAsync(qnaModel);
    }
}
