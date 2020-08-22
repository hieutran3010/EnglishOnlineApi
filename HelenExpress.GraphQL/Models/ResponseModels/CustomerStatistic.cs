namespace HelenExpress.GraphQL.Models.ResponseModels
{
    public class CustomerStatistic
    {
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        
        public double TotalPurchase { get; set; }
        public double TotalSalePrice { get; set; }
        public double TotalPayment { get; set; }
        
        public double TotalCashPayment { get; set; }
        public double TotalBankTransferPayment { get; set; }
        public double TotalDebt { get; set; }
        public double TotalBill { get; set; }
        public double TotalProfit { get; set; }
        public double TotalProfitBeforeTax { get; set; }
    }
}