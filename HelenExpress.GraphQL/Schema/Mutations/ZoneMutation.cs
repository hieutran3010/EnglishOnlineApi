#region

using System;
using System.Linq;
using System.Threading.Tasks;
using GraphQLDoorNet;
using GraphQLDoorNet.Abstracts;
using GraphQLDoorNet.Attributes;
using GraphQLDoorNet.Models;
using HelenExpress.Data.Entities;
using HelenExpress.GraphQL.Models.InputModels;
using Microsoft.EntityFrameworkCore;

#endregion

namespace HelenExpress.GraphQL.Schema.Mutations
{
    [ExtendMutation]
    public class ZoneMutation : EntityMutationBase<Zone, ZoneInput>
    {
        public ZoneMutation(IUnitOfWork unitOfWork, IInputMapper inputMapper) : base(unitOfWork, inputMapper)
        {
        }

        public override async Task<HttpStatus> Delete(Guid id)
        {
            var zoneRepository = UnitOfWork.GetRepository<Zone>();

            var zone = await zoneRepository.GetQueryable().FirstOrDefaultAsync(z => z.Id == id);
            if (zone != null)
            {
                var vendorRepository = UnitOfWork.GetRepository<Vendor>();

                var vendor = await vendorRepository.GetQueryable().FirstOrDefaultAsync(v => v.Id == zone.VendorId);
                var vendorQuotations = vendor?.VendorQuotations;
                if (vendorQuotations != null && vendorQuotations.Any())
                {
                    foreach (var vendorQuotation in vendor.VendorQuotations)
                        vendorQuotation.ZonePrices =
                            vendorQuotation.ZonePrices.Where(vq => vq.ZoneId != zone.Id).ToArray();
                    vendorRepository.Update(vendor);
                }
            }

            return await base.Delete(id);
        }
    }
}