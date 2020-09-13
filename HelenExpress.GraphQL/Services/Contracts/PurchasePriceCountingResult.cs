using System;
using HelenExpress.Data.JSONModels;

namespace HelenExpress.GraphQL.Services.Contracts
{
    public class PurchasePriceCountingResult
    {
        public double QuotationPriceInUsd { get; set; }
        public double VendorNetPriceInUsd { get; set; }
        public double FuelChargeFeeInUsd { get; set; }
        public int FuelChargeFeeInVnd { get; set; }
        public double PurchasePriceInUsd { get; set; }
        public int PurchasePriceInVnd { get; set; }
        public double PurchasePriceAfterVatInUsd { get; set; }
        public int PurchasePriceAfterVatInVnd { get; set; }
        public string ZoneName { get; set; }
        public BillQuotation[] BillQuotations { get; set; }
        public DateTime? LastUpdatedQuotation { get; set; }
        public string Service { get; set; }
        public Guid VendorId { get; set; }
    }
}