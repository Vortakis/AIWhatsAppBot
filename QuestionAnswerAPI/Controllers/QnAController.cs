using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuestionAnswerAPI.Models;
using QuestionAnswerAPI.Services;

namespace QuestionAnswerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QnAController : ControllerBase
{
    private readonly IQnAService _qnaService;
    private readonly ILogger<QnAController> _logger;

    public QnAController(IQnAService qnaService, ILogger<QnAController> logger)
    {
        _qnaService = qnaService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<QnAModel>> GetQnA([FromQuery] string question)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        stopwatch.Start();
        var answer = await _qnaService.GetQnAAsync(question);
        stopwatch.Stop();
        _logger.LogInformation($"Time took: {stopwatch.ElapsedMilliseconds}ms.");
        return Ok(answer);
    }

    [HttpPost]
    public async Task<IActionResult> AddQnA([FromBody] QnAModel qnaModel)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        stopwatch.Start();
        await _qnaService.AddQnAAsync(qnaModel);
        stopwatch.Stop();
        _logger.LogInformation($"Time took: {stopwatch.ElapsedMilliseconds}ms.");
        return Ok("Q&A added successfully.");
    }
}
