using System;

namespace HelenExpress.GraphQL.Models.ResponseModels
{
    public class VendorStatistic
    {
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public long TotalPurchase { get; set; }
        public double TotalPayment { get; set; }
        public double TotalCashPayment { get; set; }
        public double TotalBankTransferPayment { get; set; }
        public double TotalSalePrice { get; set; }
        public double TotalDebt { get; set; }
        public double TotalBill { get; set; }
        public double TotalRawProfit { get; set; }
        public double TotalRawProfitBeforeTax { get; set; }
        public double TotalProfit { get; set; }
    }
}