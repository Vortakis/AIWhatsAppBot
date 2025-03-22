using QuestionAnswerAPI.Models;

namespace QuestionAnswerAPI.Services
{
    public static class EmbeddingHelper
    {
        public static QnAModel GetByEmbedding(float[] targetEmbedding, List<QnAModel> allQnAs, float similarThreshold = 0.75f)
        {
            var bestMatch = allQnAs
                .Select(qna => new
                {
                    QnA = qna,
                    Similarity = CosineSimilarity(qna.Embedding, targetEmbedding)
                })
                .OrderByDescending(x => x.Similarity)
                .FirstOrDefault();

            if (bestMatch != null && bestMatch.Similarity >= similarThreshold)
            {
                return bestMatch.QnA;
            }

            return null!;
        }

        private static float CosineSimilarity(float[] vec1, float[] vec2)
        {
            float dotProduct = vec1.Zip(vec2, (x, y) => x * y).Sum();
            float magnitude1 = (float)Math.Sqrt(vec1.Sum(x => x * x));
            float magnitude2 = (float)Math.Sqrt(vec2.Sum(x => x * x));

            return dotProduct / (magnitude1 * magnitude2);
        }
    }
}
