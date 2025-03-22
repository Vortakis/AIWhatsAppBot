namespace AIProviderAPI.Models.DTOs;

public class AIResponseDTO
{
    public string Answer { get; set; } = string.Empty;

    public float[] Embeddings { get; set; } = Array.Empty<float>();
}
