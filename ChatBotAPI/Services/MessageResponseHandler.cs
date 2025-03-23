using AIWAB.Common.Core.AIProviderAPI.Enum;
using AIProviderAPI.Protos;
using AIWAB.Common.Core.AIProviderAPI.GrpcClients;
using AIWAB.Common.Core.QuestionAnswerAPI.GrpcClients;
using QuestionAnswerAPI.Protos;

namespace ChatBotAPI.Services
{
    public class MessageResponseHandler : IMessageResponseHandler
    {
        private readonly IAIProviderClientService _aiProviderClientService;
        private readonly IQnAClientService _qnaClientService;

        public MessageResponseHandler(
            IQnAClientService qnaClientService,
            IAIProviderClientService aiProviderClientService)
        {
            _qnaClientService = qnaClientService;
            _aiProviderClientService = aiProviderClientService;
        }

        public async Task<string> HandleMessageAsync(string message)
        {
            QuestionDTO questionDTO = new QuestionDTO { Question = message };
            var qnaDTO = await _qnaClientService.GetQnAAsync(questionDTO);
            
            if (!string.IsNullOrEmpty(qnaDTO.Answer))
            {
                return qnaDTO.Answer;
            }

            AIRequest aiRequest = new AIRequest { Prompt = message, PromptType = AIPromptType.QnA.ToString()};     
            var response = await _aiProviderClientService.PromptAIAsync(aiRequest);

            qnaDTO.Answer = response.Answer;
            await _qnaClientService.AddQnAAsync(qnaDTO);

            return response.Answer;
        }
    }
}
