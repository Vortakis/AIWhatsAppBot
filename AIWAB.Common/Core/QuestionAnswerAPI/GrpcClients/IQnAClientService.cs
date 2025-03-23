using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIProviderAPI.Protos;
using QuestionAnswerAPI.Protos;

namespace AIWAB.Common.Core.QuestionAnswerAPI.GrpcClients
{
    public interface IQnAClientService
    {
        Task<QnADTO> GetQnAAsync(QuestionDTO questionDTO);
        Task AddQnAAsync(QnADTO qnaDTO);
    }
}
