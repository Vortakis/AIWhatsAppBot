using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polly;
using Polly.Wrap;

namespace AIWAB.Common.General.HttpPolicy
{

    public class FailurePolicyHandler : DelegatingHandler
    {
        private readonly AsyncPolicyWrap<HttpResponseMessage> _policyWrap;

        public FailurePolicyHandler(IFailurePolicyService failurePolicyService)
        {
            _policyWrap = Policy.WrapAsync(
                failurePolicyService.GetRetryPolicy(),
                failurePolicyService.GetCircuitBreakerPolicy()
            );
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await _policyWrap.ExecuteAsync(() => base.SendAsync(request, cancellationToken));
        }
    }
}
