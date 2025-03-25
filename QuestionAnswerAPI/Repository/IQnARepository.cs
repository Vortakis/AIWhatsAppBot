using QuestionAnswerAPI.Models;

namespace QuestionAnswerAPI.Repository;

public interface IQnARepository
{
    bool InitialiseRepo();
    QnAModel? GetQnA(string question);

    Task AddQnAAsync(QnAModel qna);

    List<QnAModel> GetAllQnA();
}
