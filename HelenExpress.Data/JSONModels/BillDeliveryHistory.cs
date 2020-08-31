using System;

namespace HelenExpress.Data.JSONModels
{
    public class BillDeliveryHistory
    {
        public DateTime? Date { get; set; }
        public DateTime? Time { get; set; }
        public string Status { get; set; }
    }
}