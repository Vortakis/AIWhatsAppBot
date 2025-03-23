using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AIWAB.Common.Core.AIProviderAPI.Enum;

namespace AIProviderAPI.Models.DTOs
{
    public class AIRequestDTO
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AIPromptType PromptType { get; set; } = AIPromptType.QnA;

        public string Prompt { get; set; } = string.Empty;
    }
}
