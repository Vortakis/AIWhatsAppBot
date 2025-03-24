using System.Diagnostics;
using ChatBotAPI.MessagingPlatforms.Enum;
using ChatBotAPI.MessagingPlatforms.Twilio.Model;
using ChatBotAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatBotAPI.Controllers
{
    [Route("api/chatbot")]
    [ApiController]
    public class ChatBotController : ControllerBase
    {
        private readonly IChatBotService _chatBotService;
        private readonly ILogger<ChatBotController> _logger;

        public ChatBotController (IChatBotService chatBotService, ILogger<ChatBotController> logger)
        {
            _chatBotService = chatBotService;
            _logger = logger;
        }

        [HttpPost("webhook/twilio")]
        public async Task<IActionResult> ReceiveMessage([FromForm] TwilioRequest request)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            await _chatBotService.ProcessChatMessageAsync<TwilioRequest>(MessagingPlatform.Twilio, request);
            return Ok();
        }

    }
}
