using AIWAB.Common.Core.QuestionAnswerAPI.DTOs;
using QuestionAnswerAPI.Models;
using QuestionAnswerAPI.Repository;

namespace QuestionAnswerAPI.Services;

public class QnAService : IQnAService
{
    private readonly IQnARepository _qnaRepository;
    private readonly ILogger<QnAService> _logger;

    public QnAService(IQnARepository qnaRepository, ILogger<QnAService> logger)
    {
        _qnaRepository = qnaRepository;
        _logger = logger;
    }

    public string GetAnswer(string question)
    {
        var exactMatches = _qnaRepository.GetAllQnA()
            .Where(q => q.Question.Equals(question, StringComparison.OrdinalIgnoreCase)).ToList();
        if (exactMatches.Any())
            return exactMatches.First().Answer;

        return "Sorry, I don't have an answer for that.";
    }

    public void AddQnA(QnACreateDTO qnaCreateDTO)
    {
        var newQnA = new QnAModel { Question = qnaCreateDTO.Question, Answer = qnaCreateDTO.Answer };
        _qnaRepository.AddQnA(newQnA);
    }
}
