using System;
using System.Linq;
using System.Threading.Tasks;
using GraphQLDoorNet.Abstracts;
using HelenExpress.Data.Entities;
using HelenExpress.GraphQL.Services.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace HelenExpress.GraphQL.Services
{
    public class VendorService : IVendorService
    {
        public async Task UpdateQuotationAfterDeletingZone(IUnitOfWork unitOfWork, Guid vendorId, Guid zoneId)
        {
            var vendorRepository = unitOfWork.GetRepository<Vendor>();

            var vendor = await vendorRepository.GetQueryable(false).FirstOrDefaultAsync(v => v.Id == vendorId);
            var vendorQuotations = vendor?.VendorQuotations;
            if (vendorQuotations != null && vendorQuotations.Any())
            {
                foreach (var vendorQuotation in vendor.VendorQuotations)
                    vendorQuotation.ZonePrices =
                        vendorQuotation.ZonePrices.Where(vq => vq.ZoneId != zoneId).ToArray();
                vendorRepository.Update(vendor);
            }
        }
    }
}
