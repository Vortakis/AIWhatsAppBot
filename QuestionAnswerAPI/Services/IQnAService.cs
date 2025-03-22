using AIWAB.Common.Core.QuestionAnswerAPI.DTOs;
using QuestionAnswerAPI.Models;

namespace QuestionAnswerAPI.Services;

public interface IQnAService
{
    Task<QnAModel> GetAnswer(string question);

    Task AddQnAAsync(QnACreateDTO qnaCreateDTO);
}
