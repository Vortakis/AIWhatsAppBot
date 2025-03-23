namespace QuestionAnswerAPI.Models;

public class QnAModel
{
    public string Question { get; set; } = string.Empty;

    public string Answer { get; set; } = string.Empty;

    public float[] Embeddings { get; set; } = Array.Empty<float>();
}
