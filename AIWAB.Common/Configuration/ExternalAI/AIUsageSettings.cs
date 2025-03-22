using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWAB.Common.Configuration.ExternalAI;

public class AIUsageSettings
{
    public string Model { get; set; } = string.Empty;

    public string[] References { get; set; } = Array.Empty<string>();

    public float Temperature { get; set; }

    public float SimilarityThreshold { get; set; }

    public int MaxTokens { get; set; }
}
