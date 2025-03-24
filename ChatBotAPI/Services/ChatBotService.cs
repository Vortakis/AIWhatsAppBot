
using AIWAB.Common.Configuration.ExternalMsgPlatform;
using AIWAB.Common.General.MessageQueue;
using ChatBotAPI.MessagingPlatforms;
using ChatBotAPI.MessagingPlatforms.Enum;
using Microsoft.Extensions.Options;
using Twilio.Http;

namespace ChatBotAPI.Services;

public class ChatBotService : IChatBotService
{
    private readonly MessagingPlatformFactory _messagingFactory;
    private readonly IMessageResponseHandler _messageResponseHandler;
    private readonly IMessageQueue _messageQueue;
    private readonly MessagingPlatformSettings _msgPlatformSettings;

    private readonly ILogger<ChatBotService> _logger;

    public ChatBotService(
        MessagingPlatformFactory messagingFactory,
        IMessageResponseHandler messageResponseHandler,
        IMessageQueue messageQueue,
        IOptions<ExternalMsgPlatformSettings> externalMsgSettings,
        ILogger<ChatBotService> logger)
    {
        _messagingFactory = messagingFactory;
        _messageResponseHandler = messageResponseHandler;
        _msgPlatformSettings = externalMsgSettings.Value.MessagingPlatforms[externalMsgSettings.Value.DefaultPlatform];
        _logger = logger;
        _messageQueue = messageQueue;
    }

    public async Task ProcessChatMessageAsync<T>(MessagingPlatform platform, T request)
    {
        await _messageQueue.EnqueueAsync(() => ProcessMessageAsync(platform, request));
    }

    private async Task ProcessMessageAsync<T>(MessagingPlatform platform, T request)
    {
        try
        {
            var messagingService = _messagingFactory.GetService(platform);

            var incomingMessage = messagingService.ParseReceivedMessage(request);

            _logger.LogInformation($"Received message from {incomingMessage.From}: {incomingMessage.Message}");

            string outgoingMessage = await _messageResponseHandler.HandleMessageAsync(incomingMessage.Message);

            foreach (var chunk in outgoingMessage.Chunk(_msgPlatformSettings.MaxMessageLength))
            {
                string chunkString = new string(chunk);
                await messagingService.SendMessageAsync(incomingMessage.From, chunkString);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing message: {ex.Message}");

        }
    }
}
