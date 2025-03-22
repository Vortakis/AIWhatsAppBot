﻿using AIProviderAPI.Protos;
using Grpc.Core;

namespace ChatBotAPI.Clients
{
    public class AIProviderClientService : IAIProviderClientService
    {
        public AIProviderService.AIProviderServiceClient _aiProviderClient;
        public ILogger<AIProviderClientService> _logger;

        public AIProviderClientService(
            AIProviderService.AIProviderServiceClient aiProviderClient,
            ILogger<AIProviderClientService> logger)
        {
            _aiProviderClient = aiProviderClient;
            _logger = logger;
        }

        public async Task<AIResponse> PromptAIAsync(AIRequest request)
        {
            try
            {
                return await _aiProviderClient.PromptAIAsync(request);
            }
     
            catch (RpcException e)
{
                _logger.LogError($"gRPC Error: {e.StatusCode}, {e.Message}");
                throw;
            }
        }
    }
}
