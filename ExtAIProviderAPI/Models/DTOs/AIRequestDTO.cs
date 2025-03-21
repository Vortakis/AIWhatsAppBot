using System.ComponentModel.DataAnnotations;
using ExtAIProviderAPI.Models.Enum;

namespace ExtAIProviderAPI.Models.DTOs
{
    public class AIRequestDTO
    {
        public AIPromptType PromptType { get; set; } = AIPromptType.QnA;

        [Required]
        public string Prompt { get; set; } = string.Empty;
    }
}
