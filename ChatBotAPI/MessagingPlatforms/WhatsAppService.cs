
using AIWAB.Common.Configuration.MessageService;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace ChatBotAPI.MessageServices
{
    public class WhatsAppService : IMessagingPlatformService
    {

        private readonly string _twilioPhoneNumber;

        public WhatsAppService(IOptions<ExternalMsgPlatformSettings> options)
        {
            var twilioSettings = options.Value.MessagingPlatforms["Twilio"]
                ?? throw new InvalidOperationException("Twilio settings are missing.");

            _twilioPhoneNumber = twilioSettings.PhoneNumber!;
        }

        public (string From, string Message) ParseReceivedMessage(string requestBody)
        {
            var formValues = QueryHelpers.ParseQuery(requestBody);
            if (formValues.TryGetValue("From", out var fromNumber) && formValues.TryGetValue("Body", out var messageBody))
            {
                return (fromNumber.ToString(), messageBody.ToString());
            }
            throw new InvalidOperationException("Required form values are missing.");
        }

        public async Task SendMessageAsync(string to, string message)
        {
            var messageResource = await MessageResource.CreateAsync(
                       body: message,
                       from: new PhoneNumber($"whatsapp:{_twilioPhoneNumber}"),
                       to: new PhoneNumber($"whatsapp:{to}")
                   );
        }
    }
}
