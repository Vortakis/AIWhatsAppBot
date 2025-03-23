using QuestionAnswerAPI.Models;

namespace QuestionAnswerAPI.Services;

public interface IQnAService
{
    Task<QnAModel> GetQnAAsync(string question);

    Task AddQnAAsync(QnAModel qnaModel);
}
