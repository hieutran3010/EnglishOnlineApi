#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GraphQLDoorNet;
using GraphQLDoorNet.Abstracts;
using GraphQLDoorNet.Attributes;
using GraphQLDoorNet.Models;
using HelenExpress.Data.Entities;
using HelenExpress.Data.JSONModels;
using HelenExpress.GraphQL.Models.InputModels;
using HelenExpress.GraphQL.Models.ResponseModels;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

#endregion

namespace HelenExpress.GraphQL.Schema.Mutations
{
    [ExtendMutation]
    public class VendorMutation : EntityMutationBase<Vendor, VendorInput>
    {
        private readonly ILogger<VendorMutation> logger;

        public VendorMutation(IUnitOfWork unitOfWork, IInputMapper inputMapper, ILogger<VendorMutation> logger) : base(
            unitOfWork, inputMapper)
        {
            this.logger = logger;
        }

        public async Task<MutationResult> UpdateQuotation(VendorQuotationUpdateInput input)
        {
            var vendorRepository = UnitOfWork.GetRepository<Vendor>();
            var vendor = await vendorRepository.FindAsync(input.VendorId);
            if (vendor != null)
            {
                vendor.VendorQuotations = input.VendorQuotations.Adapt<VendorQuotation[]>();
                vendor.LastUpdatedQuotation = DateTime.Now;
                await UnitOfWork.SaveChangesAsync();

                return new MutationResult {DidSuccess = true};
            }

            throw new HttpRequestException("Cannot find vendor for updating");
        }

        public async Task<ServiceAssignmentResult> AssignParcelServices(Guid[] serviceIds, Guid vendorId)
        {
            var result = new ServiceAssignmentResult();

            var serviceVendorAssociationRepo = this.UnitOfWork.GetRepository<ParcelServiceVendor>();
            var zoneRepo = this.UnitOfWork.GetRepository<Zone>();

            // Checking which are new, which are delete
            var currentAssignments = await serviceVendorAssociationRepo.GetQueryable(false)
                .Where(sva => sva.VendorId == vendorId).Include(sva => sva.ParcelService).ToArrayAsync();

            var newAssignments = serviceIds.Where(si =>
                currentAssignments.FirstOrDefault(ca => ca.ParcelServiceId == si) == null);
            var deletedAssignments = currentAssignments.Where(ca => !serviceIds.Contains(ca.ParcelServiceId));

            // handle deleted assignments
            var deletedZoneIds = await this.HandleDeleteAssociatedService(deletedAssignments, vendorId,
                serviceVendorAssociationRepo,
                zoneRepo);
            var newZones = await this.HandleAssignNewServices(newAssignments, vendorId, serviceVendorAssociationRepo);

            await zoneRepo.AddRangeAsync(newZones);
            await this.UnitOfWork.SaveChangesAsync();

            result.NewZones = newZones;
            result.DeletedZoneIds = deletedZoneIds;
            return result;
        }

        private async Task<List<Guid>> HandleDeleteAssociatedService(
            IEnumerable<ParcelServiceVendor> deletedAssignments,
            Guid vendorId,
            IRepository<ParcelServiceVendor> serviceVendorAssociationRepo,
            IRepository<Zone> zoneRepository)
        {
            var deletedZoneIds = new List<Guid>();
            // remove related zones
            foreach (var deletedAssignment in deletedAssignments)
            {
                var serviceName = deletedAssignment.ParcelService.Name;
                var relatedZones = await zoneRepository.GetQueryable(false)
                    .Where(z => z.VendorId == vendorId && z.Name.StartsWith($"{serviceName}{Constants.ServiceVendorZoneSeparator}"))
                    .ToListAsync();

                deletedZoneIds.AddRange(relatedZones.Select(rz => rz.Id));

                zoneRepository.RemoveRange(relatedZones);
            }

            // remove association
            serviceVendorAssociationRepo.RemoveRange(deletedAssignments);

            return deletedZoneIds;
        }

        private async Task<List<Zone>> HandleAssignNewServices(IEnumerable<Guid> newServiceIds,
            Guid vendorId,
            IRepository<ParcelServiceVendor> serviceVendorAssociationRepo)
        {
            var parcelServiceRepo = this.UnitOfWork.GetRepository<ParcelService>();

            var newZones = new List<Zone>();

            foreach (var newServiceId in newServiceIds)
            {
                var service = await parcelServiceRepo.GetQueryable().Where(s => s.Id == newServiceId)
                    .Include(s => s.ParcelServiceZones).FirstOrDefaultAsync();
                if (service == null)
                {
                    this.logger.LogWarning($"[AssignParcelServices] Cannot find service with id {newServiceId}");
                    continue;
                }

                // add the association
                await serviceVendorAssociationRepo.AddAsync(new ParcelServiceVendor
                {
                    ParcelServiceId = newServiceId,
                    VendorId = vendorId
                });

                // add the related service zones to the vendor zone
                foreach (var serviceZone in service.ParcelServiceZones)
                {
                    newZones.Add(new Zone
                    {
                        Name = $"{service.Name}{Constants.ServiceVendorZoneSeparator}{serviceZone.Name}",
                        Countries = serviceZone.Countries,
                        VendorId = vendorId
                    });
                }
            }

            return newZones;
        }
    }
}