using System;

namespace HelenExpress.GraphQL.Models.InputModels
{
    public class LicenseBillInput
    {
        public string SaleUserId { get; set; }
        public string LicenseUserId { get; set; }
        public string AccountantUserId { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string SenderAddress { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceiverAddress { get; set; }
        public DateTime Date { get; set; }
        public string Period { get; set; }
        public string ChildBillId { get; set; }
        public string AirlineBillId { get; set; }
        public Guid? VendorId { get; set; }
        public Guid? SenderId { get; set; }
        public Guid? ReceiverId { get; set; }
        public string InternationalParcelVendor { get; set; }
        public string Description { get; set; }
        public string DestinationCountry { get; set; }
        public double WeightInKg { get; set; }
        public string Status { get; set; }
    }
}