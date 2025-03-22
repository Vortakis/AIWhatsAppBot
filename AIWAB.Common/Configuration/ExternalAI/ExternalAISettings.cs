namespace AIWAB.Common.Configuration.ExternalAI;


public class ExternalAISettings
{
    public bool Enabled { get; set; } = true;

    public string DefaultAIProvider { get; set; } = "";

    public Dictionary<string, AIProviderSettings> AIProviders { get; set; } = [];

    public Dictionary<string, AIUsageSettings> AIUsage { get; set; } = [];
}

