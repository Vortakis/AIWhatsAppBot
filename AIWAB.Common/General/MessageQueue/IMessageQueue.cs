

namespace AIWAB.Common.General.MessageQueue
{
    public interface IMessageQueue
    {
        Task EnqueueAsync(Func<Task> task);

        Task<Func<Task>> DequeueAsync(CancellationToken token);
    }
}
