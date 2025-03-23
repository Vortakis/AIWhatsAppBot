
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AIWAB.Common.General.MessageQueue
{
    public class MessageProcessorService : BackgroundService
    {
        private readonly IMessageQueue _messageQueue;
        private readonly ILogger<MessageProcessorService> _logger;
        private const int MaxConcurrentTasks = 5;

        public MessageProcessorService(
            IMessageQueue messageQueue,
            ILogger<MessageProcessorService> logger)
        {
            _messageQueue = messageQueue;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var tasks = new List<Task>();

            while (!stoppingToken.IsCancellationRequested)
            {
                // Process multiple tasks concurrently, with a limit
                for (int i = 0; i < MaxConcurrentTasks; i++)
                {
                    var taskToProcess = await _messageQueue.DequeueAsync(stoppingToken);
                    tasks.Add(ProcessTaskWithRetryAsync(taskToProcess));
                }

                await Task.WhenAny(tasks);  // Wait for any task to complete
                tasks.RemoveAll(t => t.IsCompleted);  // Remove completed tasks
            }
        }

        private async Task ProcessTaskWithRetryAsync(Func<Task> task)
        {
            int retries = 3;
            while (retries > 0)
            {
                try
                {
                    await task();
                    return; // If task succeeds, exit
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Task failed: {ex.Message}, retrying...");
                    retries--;
                    await Task.Delay(1000); // Delay before retrying
                }
            }
        }
    }
}
