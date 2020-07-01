#region

using System;
using System.ComponentModel.DataAnnotations.Schema;
using EFPostgresEngagement.DataAnnotationAttributes;

#endregion

namespace HelenExpress.Data.Entities
{
    public class Zone : EntityBase
    {
        [UniqueIndex(AdditionalColumns = "VendorId")]
        public string Name { get; set; }

        [Column(TypeName = "jsonb")] public string[] Countries { get; set; }

        public Guid VendorId { get; set; }
        public Vendor Vendor { get; set; }
    }
}