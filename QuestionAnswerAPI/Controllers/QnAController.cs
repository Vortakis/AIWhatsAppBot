using AIWAB.Common.QnA.DTOs;
using Microsoft.AspNetCore.Mvc;
using QnAServiceApi.Services;

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
    public ActionResult<string> GetAnswer([FromQuery] string question)
    {
        var answer = _qnaService.GetAnswer(question);
        return Ok(answer);
    }

    [HttpPost]
    public IActionResult AddQnA([FromBody] QnACreateDTO qnaDto)
    {
        _qnaService.AddQnA(qnaDto);
        return Ok("Q&A added successfully.");
    }
}
