namespace HelenExpress.GraphQL.Models.InputModels
{
    public class BillQuotationInput
    {
        public double? StartWeight { get; set; }
        public double EndWeight { get; set; }
        public double? PriceInUsd { get; set; }
    }
}