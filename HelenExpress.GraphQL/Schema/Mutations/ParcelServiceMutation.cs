using System;
using System.IO;
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
    public class ParcelServiceMutation : EntityMutationBase<ParcelService, ParcelServiceInput>
    {
        private readonly IVendorService vendorService;

        public ParcelServiceMutation(IUnitOfWork unitOfWork, IInputMapper inputMapper, IVendorService vendorService) : base(unitOfWork, inputMapper)
        {
            this.vendorService = vendorService;
        }

        public override async Task<HttpStatus> Delete(Guid id)
        {
            var serviceRepository = this.UnitOfWork.GetRepository<ParcelService>();
            var service = await serviceRepository.FindAsync(id);
            if (service.IsSystem)
            {
                throw new InvalidDataException("You cannot delete system service");
            }

            // get all mapped vendors
            var serviceVendorAssociationRepo = this.UnitOfWork.GetRepository<ParcelServiceVendor>();
            var zoneRepository = this.UnitOfWork.GetRepository<Zone>();
            var associatedVendors = await serviceVendorAssociationRepo.GetQueryable(false)
                .Where(av => av.ParcelServiceId == id)
                .Include(v => v.Vendor)
                .Include(v => v.Vendor.Zones)
                .Select(v => v.Vendor).ToArrayAsync();
            
            // remove the quotations
            foreach (var associatedVendor in associatedVendors)
            {
                var zones = associatedVendor.Zones;
                if (zones != null && zones.Any())
                {
                    var willBeDeletedZones = zones.Where(z =>
                        z.Name.StartsWith($"{service.Name}{Constants.ServiceVendorZoneSeparator}")).ToList();
                    foreach (var willBeDeletedZone in willBeDeletedZones)
                    {
                        await this.vendorService.UpdateQuotationAfterDeletingZone(this.UnitOfWork, associatedVendor.Id,
                            willBeDeletedZone.Id);
                    }

                    zoneRepository.RemoveRange(willBeDeletedZones);
                }
            }
            
            return await base.Delete(id);
        }
    }
}