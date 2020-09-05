using System;
using System.Collections.Generic;
using HelenExpress.Data.Entities;

namespace HelenExpress.GraphQL.Models.ResponseModels
{
    public class ServiceAssignmentResult
    {
        public List<Zone> NewZones { get; set; }
        public List<Guid> DeletedZoneIds { get; set; }
    }
}