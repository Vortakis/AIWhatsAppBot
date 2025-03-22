using AIWAB.Common.Core.QuestionAnswerAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using QuestionAnswerAPI.Models;
using QuestionAnswerAPI.Services;

namespace QuestionAnswerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QnAController : ControllerBase
{
    private readonly IQnAService _qnaService;

    public QnAController(IQnAService qnaService)
    {
        _qnaService = qnaService;
    }

    [HttpGet]
    public ActionResult<QnAModel> GetQnA([FromQuery] string question)
    {
        var answer = _qnaService.GetAnswer(question);
        return Ok(answer);
    }

    [HttpPost]
    public async Task<IActionResult> AddQnA([FromBody] QnACreateDTO qnaDto)
    {
        await Task.Run(() => _qnaService.AddQnAAsync(qnaDto));
        return Ok("Q&A added successfully.");
    }
}
