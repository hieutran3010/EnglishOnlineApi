namespace HelenExpress.GraphQL.Models.ResponseModels
{
    public class CustomerStatistic
    {
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public double TotalSalePrice { get; set; }
        public double TotalPayment { get; set; }
        public double TotalDebt { get; set; }
        public double TotalBill { get; set; }
    }
}