namespace HelenExpress.GraphQL.Models.ResponseModels
{
    public class SaleReport
    {
        public string SaleUserId { get; set; }
        public string SaleName { get; set; }
        public double? TotalSalePrice { get; set; }
        public double TotalRawProfit { get; set; }
        public double TotalRawProfitBeforeTax { get; set; }
        public double TotalProfit { get; set; }
        public int TotalBill { get; set; }
        public double TotalPurchase { get; set; }
    }
}