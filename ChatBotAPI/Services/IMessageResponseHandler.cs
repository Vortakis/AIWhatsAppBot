namespace ChatBotAPI.Services
{
    public interface IMessageResponseHandler
    {
        Task<string> HandleMessageAsync(string message);
    }
}
