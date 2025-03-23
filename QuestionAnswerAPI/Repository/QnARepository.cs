using System.Collections.Concurrent;
using Newtonsoft.Json;
using QuestionAnswerAPI.Models;
using static Grpc.Core.Metadata;

namespace QuestionAnswerAPI.Repository;

public class QnARepository : IQnARepository
{
    private readonly ILogger<QnARepository> _logger;
    private readonly string _qnaRepoFullPath;
    private readonly string _qnaRepoPath;
    private readonly string _qnaRepoFileName;
    private readonly object _fileLock = new();

    private ConcurrentDictionary<string, QnAModel> _qnaRepoData;

    public QnARepository(IWebHostEnvironment env, ILogger<QnARepository> logger)
    {
        _logger = logger;

        _qnaRepoPath = Path.Combine("/tmp", "Repository", "Data");
        _qnaRepoFileName = "qnaRepo.jsonl";
        _qnaRepoFullPath = Path.Combine(_qnaRepoPath, _qnaRepoFileName);
        _qnaRepoData = new ConcurrentDictionary<string, QnAModel>(StringComparer.OrdinalIgnoreCase);
        InitialiseRepo(env);
    }

    public QnAModel? GetQnA(string question)
    {
        _qnaRepoData.TryGetValue(question, out var qna);
        return qna;
    }


    public async Task AddQnAAsync(QnAModel qna)
    {
        if (_qnaRepoData.ContainsKey(qna.Question))
            return;

        bool added = _qnaRepoData.TryAdd(qna.Question, qna);

        if (added)
        {
            var jsonLine = JsonConvert.SerializeObject(qna);
            await File.AppendAllTextAsync(_qnaRepoFullPath, jsonLine + Environment.NewLine);
        }

        return;
    }

    public List<QnAModel> GetAllQnA()
    {
        return _qnaRepoData.Values.ToList();
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

        foreach (var line in File.ReadLines(_qnaRepoFullPath))
        {
            var entry = JsonConvert.DeserializeObject<QnAModel>(line);
            if (entry != null)
            {
                _qnaRepoData[entry.Question] = entry;
            }
        }
    }
}