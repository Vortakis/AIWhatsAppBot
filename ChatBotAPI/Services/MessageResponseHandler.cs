using AIWAB.Common.Core.AIProviderAPI.Enum;
using ChatBotAPI.Clients;
using AIProviderAPI.Protos;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace ChatBotAPI.Services
{
    public class MessageResponseHandler : IMessageResponseHandler
    {
        private readonly IAIProviderClientService _aiProviderClientService;

        public MessageResponseHandler(IAIProviderClientService aiProviderClientService)
        {
            _aiProviderClientService = aiProviderClientService;
        }

        public async Task<string> HandleMessageAsync(string message)
        {
            AIRequest aiRequest = new AIRequest { Prompt = message, PromptType = AIPromptType.QnA.ToString()};
            
            var response = await _aiProviderClientService.PromptAIAsync(aiRequest);
            return response.Answer;
        }
    }
}
