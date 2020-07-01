#region

using EFPostgresEngagement.DataAnnotationAttributes;

#endregion

namespace HelenExpress.Data.Entities
{
    public class BillDescription : EntityBase
    {
        [UniqueIndex] public string Name { get; set; }
    }
}