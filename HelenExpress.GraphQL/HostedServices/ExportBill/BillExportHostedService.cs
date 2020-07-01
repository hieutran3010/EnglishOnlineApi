using BackgroundTaskQueueNet;
using BackgroundTaskQueueNet.Abstracts;
using Microsoft.Extensions.Logging;

namespace HelenExpress.GraphQL.HostedServices.ExportBill
{
    public class BillExportHostedService: QueuedHostedService<BillExportTaskContext>
    {
        public BillExportHostedService(IBackgroundTaskQueue<BillExportTaskContext> taskQueue,
            ILogger<QueuedHostedService<BillExportTaskContext>> logger = null) : base(taskQueue, 10,
            logger)
        {
        }
    }
}