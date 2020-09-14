namespace HelenExpress.GraphQL.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Abstracts;
    using Data.Entities;
    using Contracts;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using System;
    using GraphQLDoorNet.Abstracts;
    using System.Collections.Generic;
    using Data.JSONModels;

    public class BillService : IBillService
    {
        public async Task<PurchasePriceCountingResult> CountPurchasePriceAsync(IUnitOfWork unitOfWork,
            PurchasePriceCountingParams queryParams, Vendor vendor = null)
        {
            if (queryParams == null)
            {
                throw new HttpRequestException("You should provide the countParams");
            }

            if (queryParams.IsGetLatestQuotation)
            {
                if (vendor == null)
                {
                    var vendorRepository = unitOfWork.GetRepository<Vendor>();
                    vendor = await vendorRepository.GetQueryable().Where(v => v.Id == queryParams.VendorId)
                        .Include(v => v.Zones)
                        .FirstOrDefaultAsync();
                    if (vendor == null)
                    {
                        throw new HttpRequestException("Cannot find vendor");
                    }
                }

                var zonesByCountry =
                    vendor.Zones.Where(z => z.Countries.Contains(queryParams.DestinationCountry)).ToList();
                if (!zonesByCountry.Any())
                {
                    throw new HttpRequestException(
                        $"Cannot find zone of country {queryParams.DestinationCountry} of Vendor {vendor.Name}");
                }

                var vendorServiceMappingRepo = unitOfWork.GetRepository<ParcelServiceVendor>();
                var mappedServiceNames = await vendorServiceMappingRepo.GetQueryable()
                    .Where(vs => vs.VendorId == vendor.Id)
                    .Include(v => v.ParcelService)
                    .Select(v => v.ParcelService.Name).ToArrayAsync();

                var zone = this.GetAppropriateZone(zonesByCountry, mappedServiceNames, queryParams.ServiceName);
                if (zone == null)
                {
                    throw new HttpRequestException(
                        $"Cannot find zone of country {queryParams.DestinationCountry} of Vendor {vendor.Name} with service {queryParams.ServiceName}");
                }

                return CountVendorNetPriceInUsd(vendor, queryParams, zone);
            }

            return this.CountVendorNetPriceInUsdWithCurrentQuotation(queryParams);
        }

        /// <summary>
        ///     PurchasePrice = ((NetPrice + OtherFee) * FuelChargePercent) * VAT * USD Exchange Rate
        ///     Entry 1kg NetPrice = weight * Quotation Price
        ///     Normal NetPrice = Quotation Price
        /// </summary>
        /// <param name="vendor"></param>
        /// <param name="params"></param>
        /// <param name="zone"></param>
        /// <param name="priceIncreasePercent"></param>
        /// <returns></returns>
        public PurchasePriceCountingResult CountVendorNetPriceInUsd(Vendor vendor, PurchasePriceCountingParams @params,
            Zone zone, double? priceIncreasePercent = null)
        {
            var result = new PurchasePriceCountingResult
            {
                ZoneName = zone.Name
            };

            if (vendor.VendorQuotations == null || !vendor.VendorQuotations.Any())
            {
                return result;
            }

            var orderedQuotationByWeight = vendor.VendorQuotations.OrderBy(v => v.EndWeight).ToList();

            // get quotation by weight
            var quotationByWeight =
                orderedQuotationByWeight.FirstOrDefault(vq =>
                    @params.WeightInKg <= vq.EndWeight);
            if (quotationByWeight == null)
            {
                return result;
            }

            // get quotation by zone
            var fixedZone = quotationByWeight.ZonePrices.FirstOrDefault(z => z.ZoneId == zone.Id);
            if (fixedZone == null)
            {
                return result;
            }

            var countPerOneKg = quotationByWeight.StartWeight.HasValue;
            var quotationPriceInUsd = fixedZone.PriceInUsd ?? 0;

            this.Count(@params, countPerOneKg, quotationPriceInUsd, result, priceIncreasePercent);

            var billQuotations = new List<BillQuotation>();
            foreach (var vendorQuotation in orderedQuotationByWeight)
            {
                var price = vendorQuotation.ZonePrices.FirstOrDefault(z => z.ZoneId == zone.Id);

                billQuotations.Add(new BillQuotation
                {
                    StartWeight = vendorQuotation.StartWeight,
                    EndWeight = vendorQuotation.EndWeight,
                    PriceInUsd = price?.PriceInUsd
                });
            }

            result.BillQuotations = billQuotations.ToArray();
            result.LastUpdatedQuotation = vendor.LastUpdatedQuotation;

            return result;
        }

        private PurchasePriceCountingResult CountVendorNetPriceInUsdWithCurrentQuotation(
            PurchasePriceCountingParams @params)
        {
            var result = new PurchasePriceCountingResult();

            var orderedQuotationByWeight = @params.BillQuotations.OrderBy(v => v.EndWeight).ToList();

            // get quotation by weight
            var quotationByWeight =
                orderedQuotationByWeight.FirstOrDefault(vq => vq.EndWeight >= @params.WeightInKg) ??
                orderedQuotationByWeight.Last();

            var countPerOneKg = quotationByWeight.StartWeight.HasValue;
            var quotationPriceInUsd = quotationByWeight.PriceInUsd ?? 0;
            this.Count(@params, countPerOneKg, quotationPriceInUsd, result);

            return result;
        }

        private void Count(PurchasePriceCountingParams @params, bool shouldCountPerOneKg, double quotationPriceInUsd,
            PurchasePriceCountingResult result, double? priceIncreasePercent = null)
        {
            var netPriceInUsd = !shouldCountPerOneKg
                ? quotationPriceInUsd
                : @params.WeightInKg * quotationPriceInUsd;

            var purchasePriceInUsd = netPriceInUsd + @params.OtherFeeInUsd;
            if (@params.FuelChargePercent > 0)
            {
                purchasePriceInUsd = purchasePriceInUsd * @params.FuelChargePercent;
                result.FuelChargeFeeInUsd = Math.Round(purchasePriceInUsd / 100, 4);
                result.FuelChargeFeeInVnd = (int) (result.FuelChargeFeeInUsd * @params.UsdExchangeRate);
            }

            var purchasePriceAfterVatInUsd = purchasePriceInUsd;
            if (@params.Vat.HasValue && @params.Vat.Value > 0)
            {
                purchasePriceAfterVatInUsd += purchasePriceInUsd * (@params.Vat.Value / 100);
            }

            result.PurchasePriceInUsd = Math.Round(purchasePriceInUsd, 4);
            result.PurchasePriceInVnd = (int) (purchasePriceInUsd * @params.UsdExchangeRate);

            result.PurchasePriceAfterVatInUsd = Math.Round(purchasePriceAfterVatInUsd, 4);
            result.PurchasePriceAfterVatInVnd = (int) (purchasePriceAfterVatInUsd * @params.UsdExchangeRate);
            if (priceIncreasePercent.HasValue && priceIncreasePercent.Value > 0)
            {
                result.PurchasePriceAfterVatInUsd +=
                    (result.PurchasePriceAfterVatInUsd * (priceIncreasePercent.Value / 100));
                result.PurchasePriceAfterVatInVnd =
                    (int) (result.PurchasePriceAfterVatInUsd * @params.UsdExchangeRate);
            }

            result.VendorNetPriceInUsd = Math.Round(netPriceInUsd, 4);
            result.QuotationPriceInUsd = quotationPriceInUsd;
            result.Service = @params.ServiceName;
            result.VendorId = @params.VendorId;
        }

        private Zone GetAppropriateZone(List<Zone> zonesByCountry, string[] mappedServiceNames,
            string serviceName)
        {
            Zone zone;
            if (zonesByCountry.Count == 1)
            {
                zone = zonesByCountry.First();
            }
            else
            {
                var selectedService = mappedServiceNames.FirstOrDefault(ms => ms == serviceName);
                if (selectedService == null)
                {
                    zone = zonesByCountry.FirstOrDefault(z => !z.Name.Contains(Constants.ServiceVendorZoneSeparator));
                }
                else
                {
                    zone = zonesByCountry.FirstOrDefault(z =>
                        z.Name.StartsWith($"{selectedService}{Constants.ServiceVendorZoneSeparator}"));
                }
            }

            return zone;
        }
    }
}