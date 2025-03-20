using Newtonsoft.Json;
using QuestionAnswerAPI.Models;

namespace QuestionAnswerAPI.Repository;

public class QnARepository : IQnARepository
{
    private readonly ILogger<QnARepository> _logger;
    private readonly string _qnaFilePath;


    public QnARepository(ILogger<QnARepository> logger, IWebHostEnvironment env)
    {
        _logger = logger;

        var dataPath = Path.Combine(env.WebRootPath, "data");
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }

        _qnaFilePath = Path.Combine(dataPath, "QnA.jsonl"); 

        if (!File.Exists(_qnaFilePath))
        {
            File.Create(_qnaFilePath).Dispose();
        }
    }

    public List<QnAModel> GetAllQnA()
    {
        var qnaItems = File.ReadAllLines(_qnaFilePath);
        return qnaItems
            .Select(line => JsonConvert.DeserializeObject<QnAModel>(line))
            .Where(qna => qna != null)
            .ToList()!;
    }

    public void AddQnA(QnAModel qna)
    {
        var jsonLine = JsonConvert.SerializeObject(qna);
        File.AppendAllLines(_qnaFilePath, [jsonLine]);
    }
}