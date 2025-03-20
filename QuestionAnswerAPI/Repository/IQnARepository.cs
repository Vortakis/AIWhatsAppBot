using QuestionAnswerAPI.Models;

namespace QuestionAnswerAPI.Repository;

public interface IQnARepository
{
    void AddQnA(QnAModel qnaModel);

    List<QnAModel> GetAllQnA();
}
