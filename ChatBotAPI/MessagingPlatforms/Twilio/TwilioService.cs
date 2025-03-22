
using AIWAB.Common.Configuration.ExternalMsgPlatform;
using ChatBotAPI.MessagingPlatforms.Twilio.Model;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace ChatBotAPI.MessagingPlatforms.Twilio
{
    public class TwilioService : IMessagingPlatformService
    {

        private readonly string _twilioPhoneNumber;

        public TwilioService(IOptions<ExternalMsgPlatformSettings> options)
        {
            var twilioSettings = options.Value.MessagingPlatforms["Twilio"]
                ?? throw new InvalidOperationException("Twilio settings are missing.");

            _twilioPhoneNumber = twilioSettings.PhoneNumber!;
        }

        public (string From, string Message) ParseReceivedMessage<T>(T request)
        {
            var twilioRequest = request as TwilioRequest;

            if (twilioRequest != null)
            {
                return (twilioRequest.From, twilioRequest.Body);
            }
            
            throw new InvalidOperationException("Required form values are missing.");
        }

        public async Task SendMessageAsync(string to, string message)
        {

            var messageOptions = new CreateMessageOptions(
      new PhoneNumber($"whatsapp:{to}"));
            messageOptions.From = new PhoneNumber($"whatsapp:{_twilioPhoneNumber}");
            messageOptions.Body = message;


            var sendMessageResult = await MessageResource.CreateAsync(messageOptions);
            /*var messageResource = await MessageResource.CreateAsync(
                     body: message,
                       from: new PhoneNumber($"whatsapp:{_twilioPhoneNumber}"),
                       to: new PhoneNumber($"whatsapp:{to}")
                   );*/
        }
    }
}
