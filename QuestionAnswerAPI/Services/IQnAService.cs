using AIWAB.Common.Core.QuestionAnswerAPI.DTOs;

namespace QuestionAnswerAPI.Services;

public interface IQnAService
{
    string GetAnswer(string question);

    void AddQnA(QnACreateDTO qnaCreateDTO);
}
