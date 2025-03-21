using System.ComponentModel.DataAnnotations;
using AIWAB.Common.Core.AIProviderAPI.Enum;

namespace AIProviderAPI.Models.DTOs
{
    public class AIRequestDTO
    {
        public AIPromptType PromptType { get; set; } = AIPromptType.QnA;

        [Required]
        public string Prompt { get; set; } = string.Empty;
    }
}
