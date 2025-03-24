using AIProviderAPI.AIProviders;
using AIProviderAPI.Models.DTOs;
using AIProviderAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AIProviderAPI.Controllers;

[ApiController]
[Route("api/ai")]
public class AIProviderController : ControllerBase
{
    private readonly IAIProviderService _aiProviderService;
    private readonly ILogger<AIProviderController> _logger;

    public AIProviderController(IAIProviderService aiProviderService, ILogger<AIProviderController> logger)
    {
        _aiProviderService = aiProviderService;
        _logger = logger;
    }

    [HttpPost("prompt")]
    public async Task<IActionResult> PromptAI([FromBody] AIRequestDTO request)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        stopwatch.Start();  
        var response = await _aiProviderService.ProcessPromptAsync(request);
        stopwatch.Stop();
        _logger.LogInformation($"Time took: {stopwatch.ElapsedMilliseconds}ms.");
        return Ok(response);
    }
}
