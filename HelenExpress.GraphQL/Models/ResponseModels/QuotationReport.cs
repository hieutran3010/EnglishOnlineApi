using System.Collections.Generic;

namespace HelenExpress.GraphQL.Models.ResponseModels
{
    public class QuotationReport
    {
        public string VendorName { get; set; }
        public double? OtherFeeInUsd { get; set; }
        public double? FuelChargePercent { get; set; }
        public List<QuotationReportDetail> Quotation { get; set; } = new List<QuotationReportDetail>();
    }

    public class QuotationReportDetail
    {
        public string Zone { get; set; }
        public string Service { get; set; }
        public double PurchasePriceInUsd { get; set; }
        public int PurchasePriceInVnd { get; set; }
        public double PurchasePriceAfterVatInUsd { get; set; }
        public int PurchasePriceAfterVatInVnd { get; set; }
        public double QuotationPriceInUsd { get; set; }
        public double VendorNetPriceInUsd { get; set; }
    }
}