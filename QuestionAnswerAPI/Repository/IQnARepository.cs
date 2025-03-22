using QuestionAnswerAPI.Models;

namespace QuestionAnswerAPI.Repository;

public interface IQnARepository
{
    QnAModel? GetQnA(string question);

    Task AddQnAAsync(QnAModel qna);

    List<QnAModel> GetAllQnA();
}
