using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQLDoorNet.Abstracts;
using HelenExpress.Data.Entities;
using HelenExpress.Data.JSONModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HelenExpress.GraphQL.HostedServices
{
    public class CacheBillQuotation : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public CacheBillQuotation(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = this.serviceScopeFactory.CreateScope();
            var scopedUnitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var billRepository = scopedUnitOfWork.GetRepository<Bill>();
            var vendorRepository = scopedUnitOfWork.GetRepository<Vendor>();
            var noCachingQuotationBills = await billRepository.GetQueryable()
                .Where(b => b.BillQuotations == null && b.PurchasePriceAfterVatInUsd > 0)
                .ToListAsync(cancellationToken: stoppingToken);

            foreach (var bill in noCachingQuotationBills)
            {
                var cachingQuotation = new List<BillQuotation>();

                var vendor = await vendorRepository.GetQueryable()
                    .Where(v => v.Id == bill.VendorId)
                    .Include(v => v.Zones)
                    .FirstOrDefaultAsync(cancellationToken: stoppingToken);

                var zone = vendor.Zones.FirstOrDefault(z => z.Countries.Contains(bill.DestinationCountry));
                if (zone != null && vendor.VendorQuotations != null)
                {
                    foreach (var vendorQuotation in vendor.VendorQuotations)
                    {
                        var zonePrice = vendorQuotation.ZonePrices.FirstOrDefault(z => z.ZoneId == zone.Id);
                        var quotation = new BillQuotation
                        {
                            StartWeight = vendorQuotation.StartWeight,
                            EndWeight = vendorQuotation.EndWeight,
                            PriceInUsd = zonePrice?.PriceInUsd
                        };

                        cachingQuotation.Add(quotation);
                    }

                    bill.LastUpdatedQuotation = vendor.ModifiedOn.DateTime;
                    bill.ZoneName = zone.Name;
                    bill.BillQuotations = cachingQuotation.ToArray();
                    billRepository.Update(bill);
                }

                await scopedUnitOfWork.SaveChangesAsync(stoppingToken);
            }
        }
    }
}