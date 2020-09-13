using EFPostgresEngagement.DataAnnotationAttributes;

namespace HelenExpress.Data.Entities
{
    public class SaleQuotationRate: EntityBase
    {
        [UniqueIndex(AdditionalColumns = "ToWeight")]
        public double FromWeight { get; set; }
        [SimpleIndex]
        public double? ToWeight { get; set; }
        public double Percent { get; set; }
    }
}