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
using HelenExpress.GraphQL.Services.Abstracts;
using Microsoft.EntityFrameworkCore;

#endregion

namespace HelenExpress.GraphQL.Schema.Mutations
{
    [ExtendMutation]
    public class ZoneMutation : EntityMutationBase<Zone, ZoneInput>
    {
        private readonly IVendorService vendorService;

        public ZoneMutation(IUnitOfWork unitOfWork, IInputMapper inputMapper, IVendorService vendorService) : base(unitOfWork, inputMapper)
        {
            this.vendorService = vendorService;
        }

        public override async Task<HttpStatus> Delete(Guid id)
        {
            var zoneRepository = UnitOfWork.GetRepository<Zone>();

            var zone = await zoneRepository.GetQueryable().FirstOrDefaultAsync(z => z.Id == id);
            if (zone != null)
            {
                await this.vendorService.UpdateQuotationAfterDeletingZone(this.UnitOfWork, zone.VendorId, zone.Id);
            }

            return await base.Delete(id);
        }
    }
}