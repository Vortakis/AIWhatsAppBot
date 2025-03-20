namespace AIWAB.Common.Configuration.ExternalAI;


public class ExternalAISettings
{
    public string DefaultAIProvider { get; set; } = "";

    public Dictionary<string, AIProviderSettings> AIProviders { get; set; } = [];

    public Dictionary<string, AIUsageSettings> AIUsage { get; set; } = [];
}

