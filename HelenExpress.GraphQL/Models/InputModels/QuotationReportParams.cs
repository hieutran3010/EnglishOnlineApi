using GraphQL.Conventions;

namespace HelenExpress.GraphQL.Models.InputModels
{
    [InputType]
    public class QuotationReportParams
    {
        public string DestinationCountry { get; set; }
        public double WeightInKg { get; set; }
        public int? Vat { get; set; }
        public int UsdExchangeRate { get; set; }
        public bool IsApplySaleRate { get; set; }
    }
}