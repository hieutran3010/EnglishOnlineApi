using GraphQL.Conventions;

namespace HelenExpress.GraphQL.Models.InputModels
{
    [InputType]
    public class BillQuotationInput
    {
        public double? StartWeight { get; set; }
        public double EndWeight { get; set; }
        public double? PriceInUsd { get; set; }
    }
}