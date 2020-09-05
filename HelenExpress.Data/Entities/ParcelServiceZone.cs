using System;
using System.ComponentModel.DataAnnotations.Schema;
using EFPostgresEngagement.DataAnnotationAttributes;

namespace HelenExpress.Data.Entities
{
    public class ParcelServiceZone: EntityBase
    {
        [UniqueIndex(AdditionalColumns = "ParcelServiceId")]
        public string Name { get; set; }

        [Column(TypeName = "jsonb")] public string[] Countries { get; set; }

        public Guid ParcelServiceId { get; set; }
        public ParcelService ParcelService { get; set; }
    }
}