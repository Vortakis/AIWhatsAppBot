using Newtonsoft.Json;
using QuestionAnswerAPI.Models;

namespace QuestionAnswerAPI.Repository;

public class QnARepository : IQnARepository
{
    private readonly ILogger<QnARepository> _logger;
    private readonly string _qnaRepoFullPath;
    private readonly string _qnaRepoPath;
    private readonly string _qnaRepoFileName;


    public QnARepository(IWebHostEnvironment env, ILogger<QnARepository> logger)
    {
        _logger = logger;

        _qnaRepoPath = Path.Combine(env.ContentRootPath, "Repository", "Data");
        _qnaRepoFileName = "qnaRepo.jsonl";
        _qnaRepoFullPath = Path.Combine(_qnaRepoPath, _qnaRepoFileName);

        InitialiseRepo(env);
    }

    public List<QnAModel> GetAllQnA()
    {
        var qnaItems = File.ReadAllLines(_qnaRepoFullPath);
        return qnaItems
            .Select(line => JsonConvert.DeserializeObject<QnAModel>(line))
            .Where(qna => qna != null)
            .ToList()!;
    }

    public void AddQnA(QnAModel qna)
    {
        var jsonLine = JsonConvert.SerializeObject(qna);
        File.AppendAllLines(_qnaRepoFullPath, [jsonLine]);
    }

    private void InitialiseRepo(IWebHostEnvironment env)
    {
        if (!Directory.Exists(_qnaRepoPath))
        {
            Directory.CreateDirectory(_qnaRepoPath);
        }

        if (!File.Exists(_qnaRepoFullPath))
        {
            File.Create(_qnaRepoFullPath).Dispose();
        }
    }
}