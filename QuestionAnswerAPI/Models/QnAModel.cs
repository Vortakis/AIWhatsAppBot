namespace QuestionAnswerAPI.Models;

public class QnAModel
{
    public required float[] Embedding { get; set; }

    public required string Question { get; set; }

    public required string Answer { get; set; }
}
