using AIWAB.Common.QnA.DTOs;

namespace QuestionAnswerAPI.Services;

public interface IQnAService
{
    string GetAnswer(string question);

    void AddQnA(QnACreateDTO qnaCreateDTO);
}
