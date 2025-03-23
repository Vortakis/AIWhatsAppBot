using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
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

    public override async Task<QnADTO> GetQnA(QuestionDTO questionDTO, ServerCallContext context)
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

    public override async Task<Empty> AddQnA(QnADTO qnaDTO, ServerCallContext context)
    {
        var qnaModel = new QnAModel
        {
            Embeddings = qnaDTO.Embeddings.ToArray(),
            Question = qnaDTO.Question,
            Answer = qnaDTO.Answer
        };
        await _qnaService.AddQnAAsync(qnaModel);
        return new Empty();
    }
}
