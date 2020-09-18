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

namespace HelenExpress.GraphQL.Schema.Mutations
{
    [ExtendMutation]
    public class ParcelServiceZoneMutation : EntityMutationBase<ParcelServiceZone, ParcelServiceZoneInput>
    {
        private readonly IVendorService vendorService;

        public ParcelServiceZoneMutation(IUnitOfWork unitOfWork, IInputMapper inputMapper, IVendorService vendorService) : base(unitOfWork, inputMapper)
        {
            this.vendorService = vendorService;
        }

        public override async Task<HttpStatus> Delete(Guid id)
        {
            var zoneRepository = this.UnitOfWork.GetRepository<Zone>();
            var serviceZoneRepository = this.UnitOfWork.GetRepository<ParcelServiceZone>();
            var serviceZone = await serviceZoneRepository.GetQueryable().Where(sz => sz.Id == id)
                .Include(z => z.ParcelService).FirstOrDefaultAsync();

            var associatedZones = await zoneRepository.GetQueryable(false).Where(z =>
                    z.Name ==
                    $"{serviceZone.ParcelService.Name}{Constants.ServiceVendorZoneSeparator}{serviceZone.Name}")
                .ToArrayAsync();
            foreach (var associatedZone in associatedZones)
            {
                await this.vendorService.UpdateQuotationAfterDeletingZone(this.UnitOfWork, associatedZone.VendorId,
                    associatedZone.Id);
            }

            zoneRepository.RemoveRange(associatedZones);

            return await base.Delete(id);
        }
    }
}
