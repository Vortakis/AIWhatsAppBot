
using System.Text.RegularExpressions;
using System.Text;
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

            foreach (var chunk in ChunkBySentence(outgoingMessage, _msgPlatformSettings.MaxMessageLength))
            {
                await messagingService.SendMessageAsync(incomingMessage.From, chunk);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing message: {ex.Message}");

        }
    }

    private IEnumerable<string> ChunkBySentence(string text, int maxLength)
    {
        // Split by both sentences (end punctuation + space) and newlines.
        var chunks = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

        var currentChunk = new StringBuilder();

        foreach (var line in chunks)
        {
            string cleanLine = line.Trim();

            // If adding this line exceeds the maxLength, yield the current chunk
            if (currentChunk.Length + cleanLine.Length + 1 > maxLength)
            {
                yield return currentChunk.ToString().Trim();
                currentChunk.Clear();
            }

            // Add the line to the current chunk
            currentChunk.AppendLine(cleanLine);
        }

        // If there's anything left in the current chunk, yield it
        if (currentChunk.Length > 0)
            yield return currentChunk.ToString().Trim();
    }
}
