using AIWAB.Common.Configuration.ExternalAI;
using ExtAIProviderAPI.AIProviders.OpenAI;
using Microsoft.Extensions.Options;

namespace ExtAIProviderAPI.AIProviders
{
    public class AIProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _defaultAIProvider;

        public AIProviderFactory(IServiceProvider serviceProvider, IOptions<ExternalAISettings> externalAISettings)
        {
            _serviceProvider = serviceProvider;
            _defaultAIProvider = externalAISettings.Value.DefaultAIProvider;
        }

        public IAIProvider GetProvider(string? aiProvider = null)
        {
            string providerName = aiProvider ?? _defaultAIProvider;

            return providerName switch
            {
                "OpenAI" => _serviceProvider.GetRequiredService<OpenAIProvider>(),
                _ => throw new NotImplementedException($"AI Provider '{providerName}' is not identified/implemented.")
            };
        }

    }
}
