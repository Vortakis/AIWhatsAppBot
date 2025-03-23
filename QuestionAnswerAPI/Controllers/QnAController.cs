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
    public async Task<ActionResult<QnAModel>> GetQnA([FromQuery] string question)
    {
        var answer = await _qnaService.GetQnAAsync(question);
        return Ok(answer);
    }

    [HttpPost]
    public async Task<IActionResult> AddQnA([FromBody] QnAModel qnaModel)
    {
        await _qnaService.AddQnAAsync(qnaModel);
        return Ok("Q&A added successfully.");
    }
}
