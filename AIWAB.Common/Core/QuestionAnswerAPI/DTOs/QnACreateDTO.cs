namespace AIWAB.Common.Core.QuestionAnswerAPI.DTOs;

public class QnACreateDTO
{
    public float[] Embedding { get; set; } = Array.Empty<float>();

    public string Question { get; set; } = string.Empty;

    public string Answer { get; set; } = string.Empty;
}
