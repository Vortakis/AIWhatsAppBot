
using ChatBotAPI.MessagingPlatforms;
using ChatBotAPI.MessagingPlatforms.Enum;

namespace ChatBotAPI.Services
{
    public class ChatBotService : IChatBotService
    {
        private readonly MessagingPlatformFactory _messagingFactory;
        private readonly IMessageResponseHandler _messageResponseHandler;
        private readonly ILogger<ChatBotService> _logger;

        public ChatBotService(
            MessagingPlatformFactory messagingFactory,
            IMessageResponseHandler messageResponseHandler,
            ILogger<ChatBotService> logger)
        {
            _messagingFactory = messagingFactory;
            _messageResponseHandler = messageResponseHandler;
            _logger = logger;
        }

        public async Task ProcessChatMessageAsync<T>(MessagingPlatform platform, T request)
        {
            var messagingService = _messagingFactory.GetService(platform);

            var incomingMessage = messagingService.ParseReceivedMessage(request);

            _logger.LogInformation($"Received message from {incomingMessage.From}: {incomingMessage.Message}");

            string outgoingMessage = await _messageResponseHandler.HandleMessageAsync(incomingMessage.Message);

            await messagingService.SendMessageAsync(incomingMessage.From, outgoingMessage);
        }
    }
}
