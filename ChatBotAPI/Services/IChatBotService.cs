using ChatBotAPI.MessagingPlatforms.Enum;

namespace ChatBotAPI.Services
{
    public interface IChatBotService
    {
        Task ProcessChatMessageAsync<T>(MessagingPlatform platform, T request);

    }
}
