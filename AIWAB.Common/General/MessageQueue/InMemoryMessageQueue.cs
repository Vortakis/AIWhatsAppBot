
using System.Threading.Channels;


namespace AIWAB.Common.General.MessageQueue
{
    public class InMemoryMessageQueue : IMessageQueue
    {
        private readonly Channel<Func<Task>> _queue;

        public InMemoryMessageQueue()
        {
            _queue = Channel.CreateUnbounded<Func<Task>>();
        }

        public async Task EnqueueAsync(Func<Task> task)
        {
            await _queue.Writer.WriteAsync(task);
        }

        // Dequeue a task
        public async Task<Func<Task>> DequeueAsync(CancellationToken token)
        {
            return await _queue.Reader.ReadAsync(token);
        }
    }
}
