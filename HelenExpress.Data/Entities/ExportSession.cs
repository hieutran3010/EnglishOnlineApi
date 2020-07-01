using EFPostgresEngagement.DataAnnotationAttributes;

namespace HelenExpress.Data.Entities
{
    public class ExportSession: EntityBase
    {
        [UniqueIndex(AdditionalColumns = "ExportType")]
        public string UserId { get; set; }
        public string Status { get; set; }
        public string FilePath { get; set; }
        public string ExportType { get; set; }
        public string Note { get; set; }
    }
}