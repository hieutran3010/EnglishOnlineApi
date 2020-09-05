using System;
using EFPostgresEngagement.DataAnnotationAttributes;

namespace HelenExpress.Data.Entities
{
    public class ParcelServiceVendor : EntityBase
    {
        [UniqueIndex(AdditionalColumns = "VendorId")]
        public Guid ParcelServiceId { get; set; }
        public ParcelService ParcelService { get; set; }
        [SimpleIndex] public Guid VendorId { get; set; }
        public Vendor Vendor { get; set; }
    }
}
