using System;

namespace HelenExpress.GraphQL.Models.ResponseModels
{
    public class VendorStatistic
    {
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public double TotalPurchase { get; set; }
        public double TotalPayment { get; set; }
        public double TotalDebt { get; set; }
    }
}