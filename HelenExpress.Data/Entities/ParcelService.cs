using System.Collections.Generic;
using EFPostgresEngagement.DataAnnotationAttributes;

namespace HelenExpress.Data.Entities
{
    public class ParcelService: EntityBase
    {
        [UniqueIndex]
        public string Name { get; set; }

        public bool IsSystem { get; set; }

        public List<ParcelServiceZone> ParcelServiceZones { get; set; }
        public List<ParcelServiceVendor> ParcelServiceVendors { get; set; }
    }
}