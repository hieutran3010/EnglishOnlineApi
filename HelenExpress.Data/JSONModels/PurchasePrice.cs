namespace HelenExpress.Data.JSONModels
{
    public class PurchasePrice
    {
        public double? PurchasePriceInUsd { get; set; }
        public int? PurchasePriceInVnd { get; set; }
        public double? PurchasePriceAfterVatInUsd { get; set; }
        public int? PurchasePriceAfterVatInVnd { get; set; }
        public double? VendorNetPriceInUsd { get; set; }
        public double? QuotationPriceInUsd { get; set; }
    }
}