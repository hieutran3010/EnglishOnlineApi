using BackgroundTaskQueueNet;

namespace HelenExpress.GraphQL.HostedServices.ExportBill
{
    public class BillExportTaskContext: BackgroundTaskContext
    {
        public string UserId { get; set; }
        public string Query { get; set; }
        public string Note { get; set; }
    }
}