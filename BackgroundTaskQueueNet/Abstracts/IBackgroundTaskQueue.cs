using System.Threading;
using System.Threading.Tasks;

namespace BackgroundTaskQueueNet.Abstracts
{
    public interface IBackgroundTaskQueue<TContext> where TContext : BackgroundTaskContext
    {
        /// <summary>
        /// Queue a background work item
        /// </summary>
        /// <param name="context">The task context</param>
        void QueueBackgroundWorkItem(TContext context);

        /// <summary>
        /// Dequeue a work item
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task<TContext> DequeueAsync(
            CancellationToken cancellationToken);

        /// <summary>
        /// Cancel background tasks by session id
        /// </summary>
        /// <param name="sessionId">The session id</param>
        Task CancelBySession(string sessionId);

        /// <summary>
        /// Clean up context after each task runs complete
        /// </summary>
        /// <param name="context">The task context</param>
        void CleanUp(TContext context);
    }
}