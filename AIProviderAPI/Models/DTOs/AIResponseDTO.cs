namespace AIProviderAPI.Models.DTOs;

public class AIResponseDTO
{
    public string? Answer { get; set; }

    public float[]? Embeddings { get; set; }
}
