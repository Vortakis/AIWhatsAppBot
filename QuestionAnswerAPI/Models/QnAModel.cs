namespace QuestionAnswerAPI.Models;

public class QnAModel
{
    public float[] Embeddings { get; set; } = Array.Empty<float>();

    public string Question { get; set; } = string.Empty;

    public string Answer { get; set; } = string.Empty;
}
