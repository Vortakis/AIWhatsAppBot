namespace ChatBotAPI.MessageServices
{
    public interface IMessagingPlatformService
    {
        Task SendMessageAsync(string to, string message);
        (string From, string Message) ParseReceivedMessage<T>(T request);
    }
}
