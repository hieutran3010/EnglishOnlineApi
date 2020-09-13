using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQLDoorNet;
using GraphQLDoorNet.Abstracts;
using GraphQLDoorNet.Attributes;
using HelenExpress.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelenExpress.GraphQL.Schema.Queries
{
    [ExtendQuery]
    public class ZoneQuery : EntityQueryBase<Zone>
    {
        public ZoneQuery(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<Zone>> GetZoneByVendorAndCountry(Guid vendorId, string destinationCountry)
        {
            var zoneRepository = this.UnitOfWork.GetRepository<Zone>();
            var zones = await zoneRepository.GetQueryable()
                .Where(z => z.VendorId == vendorId).ToListAsync();

            var adaptedZoneWithCountry = zones.Where(z => z.Countries.Contains(destinationCountry)).ToList();

            return adaptedZoneWithCountry;
        }
    }
}