using AIWAB.Common.QnA.DTOs;

namespace QnAServiceApi.Services;

public interface IQnAService
{
    string GetAnswer(string question);

    void AddQnA(QnACreateDTO qnaCreateDTO);
}
