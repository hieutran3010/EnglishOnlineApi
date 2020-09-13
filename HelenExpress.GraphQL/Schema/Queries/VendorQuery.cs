using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQLDoorNet;
using GraphQLDoorNet.Abstracts;
using GraphQLDoorNet.Attributes;
using HelenExpress.Data.Entities;
using HelenExpress.GraphQL.Comparers;
using HelenExpress.GraphQL.Models.InputModels;
using HelenExpress.GraphQL.Models.ResponseModels;
using HelenExpress.GraphQL.Services.Abstracts;
using HelenExpress.GraphQL.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HelenExpress.GraphQL.Schema.Queries
{
    [ExtendQuery]
    public class VendorQuery : EntityQueryBase<Vendor>
    {
        private readonly IBillService billService;

        public VendorQuery(IUnitOfWork unitOfWork, IBillService billService) : base(unitOfWork)
        {
            this.billService = billService;
        }

        public async Task<List<QuotationReport>> GetQuotationReport(QuotationReportParams queryParams)
        {
            var vendorRepository = this.UnitOfWork.GetRepository<Vendor>();
            var availableVendors = await vendorRepository.GetQueryable()
                .Include(v => v.Zones)
                .Where(v => !v.IsStopped && v.Zones != null && v.Zones.Any())
                .ToListAsync();

            var vendorHasQuotations =
                availableVendors.Where(v =>
                    v.VendorQuotations != null && v.VendorQuotations.Any() &&
                    v.Zones.Any(z => z.Countries.Contains(queryParams.DestinationCountry))).ToList();

            var increasePercent = await this.GetIncreasePercent(queryParams);

            var result = new List<QuotationReport>();
            foreach (var vendor in vendorHasQuotations)
            {
                var report = new QuotationReport
                {
                    VendorName = vendor.Name
                };

                var zonesByCountry = vendor.Zones.Where(z => z.Countries.Contains(queryParams.DestinationCountry))
                    .ToList();
                foreach (var zone in zonesByCountry)
                {
                    var reportDetail = new List<QuotationReportDetail>();

                    var zoneSplit = this.SplitZoneName(zone.Name);

                    var countingParams = new PurchasePriceCountingParams
                    {
                        Vat = queryParams.Vat,
                        UsdExchangeRate = queryParams.UsdExchangeRate,
                        WeightInKg = queryParams.WeightInKg,
                        DestinationCountry = queryParams.DestinationCountry
                    };
                    var price = this.billService.CountVendorNetPriceInUsd(vendor, countingParams, zone,
                        increasePercent);
                    if (price.PurchasePriceInUsd > 0)
                    {
                        reportDetail.Add(new QuotationReportDetail
                        {
                            Service = zoneSplit.serviceName,
                            Zone = zoneSplit.zoneName,
                            PurchasePriceInUsd = price.PurchasePriceInUsd,
                            PurchasePriceInVnd = price.PurchasePriceInVnd,
                            PurchasePriceAfterVatInUsd = price.PurchasePriceAfterVatInUsd,
                            PurchasePriceAfterVatInVnd = price.PurchasePriceAfterVatInVnd
                        });
                    }

                    if (reportDetail.Any())
                    {
                        report.Quotation.AddRange(reportDetail.OrderBy(rd => rd.PurchasePriceInUsd));
                    }
                }

                if (report.Quotation.Any())
                {
                    result.Add(report);
                }
            }

            return result.OrderBy(r => r.Quotation, new QuotationReportComparer()).ToList();
        }

        private async Task<double?> GetIncreasePercent(QuotationReportParams queryParams)
        {
            if (!queryParams.IsApplySaleRate)
            {
                return null;
            }

            var weight = queryParams.WeightInKg;
            var saleRateRepo = this.UnitOfWork.GetRepository<SaleQuotationRate>();
            var saleRate = await saleRateRepo.GetQueryable().OrderBy(sr => sr.FromWeight)
                .FirstOrDefaultAsync(sr =>weight>=sr.FromWeight &&  (weight <= sr.ToWeight || sr.ToWeight == null));

            return saleRate?.Percent;
        }

        private (string serviceName, string zoneName) SplitZoneName(string zoneName)
        {
            if (!zoneName.Contains(Constants.ServiceVendorZoneSeparator))
            {
                return (null, zoneName);
            }

            var fragments = zoneName.Split(Constants.ServiceVendorZoneSeparator);

            return (fragments[0], fragments[1]);
        }
    }
}