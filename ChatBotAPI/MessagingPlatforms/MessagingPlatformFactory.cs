using AIWAB.Common.Configuration.ExternalAI;
using ChatBotAPI.MessageServices;
using ChatBotAPI.MessagingPlatforms.Twilio;
using ChatBotAPI.MessagingServices.Enum;

namespace ChatBotAPI.MessagingServices
{
    public class MessagingPlatformFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public MessagingPlatformFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IMessagingPlatformService GetService(MessagingPlatform messagingPlatform)
        {
            return messagingPlatform switch
            {
                MessagingPlatform.Twilio => _serviceProvider.GetRequiredService<TwilioService>(),
                // Add other platforms as needed
                _ => throw new InvalidOperationException($"Unsupported platform: {messagingPlatform}")
            };
        }
    }
}
