using ExtAIProviderAPI.AIProviders;
using ExtAIProviderAPI.Models;
using ExtAIProviderAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExtAIProviderAPI.Controllers;

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
    public async Task<IActionResult> PromptAI([FromBody] AIRequest request)
    {
        var response = await _aiProviderService.ProcessPromptAsync(request);

        return Ok(response);
    }
}
