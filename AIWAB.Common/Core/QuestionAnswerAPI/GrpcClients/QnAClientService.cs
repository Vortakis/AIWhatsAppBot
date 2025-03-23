using System;
using System.Collections.Generic;

using AIProviderAPI.Protos;
using AIWAB.Common.Core.AIProviderAPI.GrpcClients;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using QuestionAnswerAPI.Protos;

namespace AIWAB.Common.Core.QuestionAnswerAPI.GrpcClients
{
    public class QnAClientService : IQnAClientService
    {
        public QnAService.QnAServiceClient _qnaClient;
        public ILogger<QnAClientService> _logger;

        public QnAClientService(
            QnAService.QnAServiceClient qnaClient,
            ILogger<QnAClientService> logger)
        {
            _qnaClient = qnaClient;
            _logger = logger;
        }

        public async Task AddQnAAsync(QnADTO qnaDTO)
        {
            try
            {
                await _qnaClient.AddQnAAsync(qnaDTO);
            }

            catch (RpcException e)
            {
                _logger.LogError($"gRPC Error: {e.StatusCode}, {e.Message}");
                throw;
            }
        }

        public async Task<QnADTO> GetQnAAsync(QuestionDTO questionDTO)
        {
            try
            {
                return await _qnaClient.GetQnAAsync(questionDTO);
            }

            catch (RpcException e)
            {
                _logger.LogError($"gRPC Error: {e.StatusCode}, {e.Message}");
                throw;
            }
        }
    }
}
