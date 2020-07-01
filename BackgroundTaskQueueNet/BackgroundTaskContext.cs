using System;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundTaskQueueNet
{
    public class BackgroundTaskContext
    {
        public Func<CancellationToken, BackgroundTaskContext, Task> WorkItem { get; set; }
        public string SessionId { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
}