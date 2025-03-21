using ChatBotAPI.MessageServices;
using ChatBotAPI.MessagingServices;
using ChatBotAPI.MessagingServices.Enum;

namespace ChatBotAPI.Services
{
    public class ChatBotService : IChatBotService
    {
        private readonly MessagingPlatformFactory _messagingFactory;
        private readonly ILogger<ChatBotService> _logger;

        public ChatBotService(MessagingPlatformFactory messagingFactory, ILogger<ChatBotService> logger)
        {
            _messagingFactory = messagingFactory;
            _logger = logger;
        }

        public async Task ProcessChatMessageAsync(MessagingPlatform platform, string requestBody, Dictionary<string, string> headers)
        {         
            var messagingService = _messagingFactory.GetService(platform);

            var messageResult = messagingService.ParseReceivedMessage(requestBody);

            _logger.LogInformation($"Received message from {messageResult.From}: {messageResult.Message}");

           // string responseMessage = await HandleMessageLogicAsync(messageResult.Message);

           // await messagingService.SendMessageAsync(messageResult.From, responseMessage);
        }
    }
}
