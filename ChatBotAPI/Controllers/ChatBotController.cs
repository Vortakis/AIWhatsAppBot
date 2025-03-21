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

        public ChatBotController (IChatBotService chatBotService)
        {
            _chatBotService = chatBotService;
        }

        [HttpPost("webhook/twilio")]
        public async Task<IActionResult> ReceiveMessage([FromForm] TwilioRequest request)
        {
            await _chatBotService.ProcessChatMessageAsync<TwilioRequest>(MessagingPlatform.Twilio, request);
            return Ok();
        }

    }
}
