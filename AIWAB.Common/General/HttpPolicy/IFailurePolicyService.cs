using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polly;

namespace AIWAB.Common.General.HttpPolicy
{
    public interface IFailurePolicyService
    {
        IAsyncPolicy<HttpResponseMessage> GetRetryPolicy();

        IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy();

    }
}
