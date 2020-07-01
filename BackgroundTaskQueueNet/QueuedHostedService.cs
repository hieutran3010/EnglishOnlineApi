using System;
using System.Threading;
using System.Threading.Tasks;
using BackgroundTaskQueueNet.Abstracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BackgroundTaskQueueNet
{
    public class QueuedHostedService<T> : BackgroundService where T : BackgroundTaskContext
    {
        private SemaphoreSlim semaphore;

        public QueuedHostedService(IBackgroundTaskQueue<T> taskQueue, int maxDegreeOfParallelism,
            ILogger<QueuedHostedService<T>> logger = null)
        {
            this.taskQueue = taskQueue;
            this.logger = logger;
            this.maxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        private readonly ILogger<QueuedHostedService<T>> logger;

        private readonly int maxDegreeOfParallelism;

        private readonly IBackgroundTaskQueue<T> taskQueue;

        private void ConfigureParallelism()
        {
            semaphore = this.maxDegreeOfParallelism > 0
                ? new SemaphoreSlim(this.maxDegreeOfParallelism - 1, this.maxDegreeOfParallelism)
                : new SemaphoreSlim(0);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            ConfigureParallelism();
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            this.logger?.LogInformation("Queued Hosted Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var context = await taskQueue.DequeueAsync(stoppingToken);

                try
                {
                    if (context != null)
                    {
                        _ = context.WorkItem(context.CancellationToken, context)
                            .ContinueWith(state =>
                            {
                                taskQueue.CleanUp(context);
                                semaphore.Release();
                                if (state.Exception != null)
                                {
                                    this.logger?.LogError(
                                        $"Error occurred executing {nameof(context)}. Ex = {state.Exception}");
                                }
                            }, stoppingToken);

                        await semaphore.WaitAsync(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    this.logger?.LogError($"Error occurred executing {nameof(context)}. Ex = {ex.Message}");
                }
            }

            this.logger?.LogInformation("Queued Hosted Service is stopping.");
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            semaphore.Release();
            return base.StopAsync(cancellationToken);
        }
    }
}