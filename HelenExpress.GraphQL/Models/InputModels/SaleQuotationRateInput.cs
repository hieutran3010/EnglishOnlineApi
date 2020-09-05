using GraphQL.Conventions;
using GraphQL.Conventions.Attributes.Execution.Wrappers;

namespace HelenExpress.GraphQL.Models.InputModels
{
    [InputType]
    public class SaleQuotationRateInput: OptionalWrapper
    {
        public double FromWeight { get; set; }
        public double? ToWeight { get; set; }
        public double Percent { get; set; }
    }
}