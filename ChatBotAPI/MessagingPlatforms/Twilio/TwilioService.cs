﻿
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
        private readonly ILogger<TwilioService> _logger;

        public TwilioService(IOptions<ExternalMsgPlatformSettings> options, ILogger<TwilioService> logger)
        {
            var twilioSettings = options.Value.MessagingPlatforms["Twilio"]
                ?? throw new InvalidOperationException("Twilio settings are missing.");

            _twilioPhoneNumber = twilioSettings.PhoneNumber!;
            _logger = logger;
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
            string twilioNumber = $"whatsapp:{_twilioPhoneNumber}";

            _logger.LogInformation($"Twilio attempt sending message To: {to} From: {twilioNumber} Message: {message}");

            var messageOptions = new CreateMessageOptions(new PhoneNumber(to));
            messageOptions.From = new PhoneNumber(twilioNumber);
            messageOptions.Body = message;
            var sendMessageResult = await MessageResource.CreateAsync(messageOptions);
        }
    }
}
