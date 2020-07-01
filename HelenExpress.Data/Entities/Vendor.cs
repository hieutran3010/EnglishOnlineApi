#region

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EFPostgresEngagement.DataAnnotationAttributes;
using HelenExpress.Data.JSONModels;

#endregion

namespace HelenExpress.Data.Entities
{
    public class Vendor : EntityBase
    {
        [UniqueIndex] public string Name { get; set; }
        public string OfficeAddress { get; set; }
        public string Phone { get; set; }
        public double? OtherFeeInUsd { get; set; }
        public double? FuelChargePercent { get; set; }
        public bool IsStopped { get; set; }
        [Column(TypeName = "jsonb")] public VendorQuotation[] VendorQuotations { get; set; }
        public List<Zone> Zones { get; set; } = new List<Zone>();
        public Bill[] Bills { get; set; }
    }
}