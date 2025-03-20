
using QnAServiceApi.Models;

namespace QnAServiceApi.Repository;

public interface IQnARepository
{
    void AddQnA(QnAModel qnaModel);

    List<QnAModel> GetAllQnA();
}
