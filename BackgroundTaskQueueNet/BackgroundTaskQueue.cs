using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BackgroundTaskQueueNet.Abstracts;
using Microsoft.Extensions.Logging;

namespace BackgroundTaskQueueNet
{
    public class BackgroundTaskQueue<TContext> : IBackgroundTaskQueue<TContext> where TContext : BackgroundTaskContext
    {
        public BackgroundTaskQueue(ILogger<BackgroundTaskQueue<TContext>> logger)
        {
            this.logger = logger;
        }

        private readonly SemaphoreSlim signal = new SemaphoreSlim(0);

        private readonly ConcurrentQueue<TContext> workItems = new ConcurrentQueue<TContext>();

        private readonly ConcurrentDictionary<string, CancellationTokenSource> cancellationTokenSources =
            new ConcurrentDictionary<string, CancellationTokenSource>();

        private readonly ILogger<BackgroundTaskQueue<TContext>> logger;

        public void QueueBackgroundWorkItem(TContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!string.IsNullOrWhiteSpace(context.SessionId))
            {
                this.cancellationTokenSources.TryGetValue(context.SessionId, out var cancellation);
                if (cancellation == null)
                {
                    cancellation = new CancellationTokenSource();
                    this.cancellationTokenSources.TryAdd(context.SessionId, cancellation);
                }

                context.CancellationToken = cancellation.Token;
            }
            else
            {
                this.logger.LogWarning(
                    "[QueueBackgroundWorkItem]Cannot set cancel token because session id is NULL");
            }

            workItems.Enqueue(context);
            signal.Release();
        }

        public async Task<TContext> DequeueAsync(CancellationToken cancellationToken)
        {
            await signal.WaitAsync(cancellationToken);
            workItems.TryDequeue(out var context);
            return context;
        }

        public Task CancelBySession(string sessionId)
        {
            return Task.Run(() =>
            {
                this.cancellationTokenSources.TryRemove(sessionId, out var cancellationTokenSource);
                if (cancellationTokenSource != null)
                {
                    this.logger.LogInformation("[CancelBySession] Start to cancel for session [{0}]", sessionId);
                    cancellationTokenSource.Cancel();
                    cancellationTokenSource.Dispose();
                }
                else
                {
                    this.logger.LogWarning("[CancelBySession] Cannot find cancellation token source of session [{0}]", sessionId);
                }
            });
        }

        public void CleanUp(TContext context)
        {
            if (context == null || string.IsNullOrWhiteSpace(context.SessionId))
            {
                this.logger.LogWarning("[CleanUp]Session id null");
                return;
            }

            var existTask = this.workItems.FirstOrDefault(workItem => workItem.SessionId == context.SessionId);
            if (existTask == null)
            {
                this.cancellationTokenSources.TryRemove(context.SessionId, out var cancellationTokenSource);
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Dispose();
                }
                else
                {
                    this.logger.LogWarning("[{0}][CleanUp]Session id has not any cancellation token source", context.SessionId);
                }
            }
            else
            {
                this.logger.LogWarning("[{0}][CleanUp]Session still have task, cannot clean", context.SessionId);
            }
        }
    }
}