using QuestionAnswerAPI.Repository;

namespace QuestionAnswerAPI.Services;

public class SearchService : ISearchService
{
    private readonly IQnARepository _qnaRepository;

    private readonly ILogger<SearchService> _logger;

    public SearchService(
        IQnARepository qnaRepository, 
        ILogger<SearchService> logger)
    {
        _qnaRepository = qnaRepository;
        _logger = logger;
    }


    public string SearchQuestion(string question)
    {
        var qnaList = _qnaRepository.GetAllQnA();
        var matchedQnA = qnaList
        .Where(qna => IsMatch(qna.Question, question))  
        .OrderByDescending(qna => GetMatchScore(qna.Question, question))  
        .FirstOrDefault();

        if (matchedQnA != null)
        {
            return matchedQnA.Answer;
        }

        return "Sorry, no relevant answer found. Please contact OpenAI for further assistance.";
    }

    private bool IsMatch(string storedQuestion, string inputQuestion)
    {
        // A very basic similarity check (can be improved with NLP libraries like FuzzyString or Semantic Search)
        return storedQuestion.ToLower().Contains(inputQuestion.ToLower());
    }

    // Calculate a simple match score (can be replaced with a more advanced scoring method)
    private double GetMatchScore(string storedQuestion, string inputQuestion)
    {
        // This is a simple way to score based on string length match (can be more sophisticated)
        var commonWords = storedQuestion.Split(' ').Intersect(inputQuestion.Split(' ')).Count();
        return commonWords;
    }
}
