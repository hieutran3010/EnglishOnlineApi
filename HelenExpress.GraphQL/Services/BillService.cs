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

    public class BillService : IBillService
    {
        public async Task<PurchasePriceCountingResult> CountPurchasePriceAsync(IUnitOfWork unitOfWork, PurchasePriceCountingParams queryParams, Vendor vendor = null)
        {
            if (queryParams == null) 
            {
                throw new HttpRequestException("You should provide the countParams");
            }

            if (vendor == null) {
                var vendorRepository = unitOfWork.GetRepository<Vendor>();
                vendor = await vendorRepository.GetQueryable().Where(v => v.Id == queryParams.VendorId)
                    .Include(v => v.Zones)
                    .FirstOrDefaultAsync();
                if (vendor == null) 
                {
                    throw new HttpRequestException("Cannot find vendor");
                }
            }

            var zoneByCountry = vendor.Zones.FirstOrDefault(z => z.Countries.Contains(queryParams.DestinationCountry));
            if (zoneByCountry == null)
            {
                throw new HttpRequestException(
                    $"Cannot find zone of country {queryParams.DestinationCountry} of Vendor {vendor.Name}");
            }

            return CountVendorNetPriceInUsd(vendor, queryParams, zoneByCountry);
        }

        /// <summary>
        ///     PurchasePrice = ((NetPrice + OtherFee) * FuelChargePercent) * VAT * USD Exchange Rate
        ///     Entry 1kg NetPrice = weight * Quotation Price
        ///     Normal NetPrice = Quotation Price
        /// </summary>
        /// <param name="vendor"></param>
        /// <param name="params"></param>
        /// <param name="zone"></param>
        /// <returns></returns>
        private PurchasePriceCountingResult CountVendorNetPriceInUsd(Vendor vendor, PurchasePriceCountingParams @params,
            Zone zone)
        {
            var result = new PurchasePriceCountingResult {
                ZoneName = zone.Name
            };

            if (vendor.VendorQuotations == null || !vendor.VendorQuotations.Any()) 
            {
                return result;
            }

            var orderedQuotationByWeight = vendor.VendorQuotations.OrderBy(v => v.EndWeight).ToList();

            // get quotation by weight
            var quotationByWeight =
                orderedQuotationByWeight.FirstOrDefault(vq => vq.EndWeight >= @params.WeightInKg) ??
                orderedQuotationByWeight.Last();

            // get quotation by zone
            var fixedZone = quotationByWeight.ZonePrices.FirstOrDefault(z => z.ZoneId == zone.Id);
            if (fixedZone == null) 
            {
                return result;
            };

            var countPerOneKg = quotationByWeight.StartWeight.HasValue;
            var quotationPriceInUsd = fixedZone.PriceInUsd ?? 0;
            var netPriceInUsd = !countPerOneKg
                ? quotationPriceInUsd
                : @params.WeightInKg * quotationPriceInUsd;

            var purchasePriceInUsd = (netPriceInUsd + @params.OtherFeeInUsd) * @params.FuelChargePercent;
            result.FuelChargeFeeInUsd = Math.Round(purchasePriceInUsd / 100, 4);
            result.FuelChargeFeeInVnd = (int) (result.FuelChargeFeeInUsd * @params.UsdExchangeRate);

            var purchasePriceAfterVatInUsd = purchasePriceInUsd;
            if (@params.Vat.HasValue) 
            {
                purchasePriceAfterVatInUsd += purchasePriceInUsd * (@params.Vat.Value / 100);
            }

            result.PurchasePriceInUsd = Math.Round(purchasePriceInUsd, 4);
            result.PurchasePriceInVnd = (int) (purchasePriceInUsd * @params.UsdExchangeRate);

            result.PurchasePriceAfterVatInUsd = Math.Round(purchasePriceAfterVatInUsd, 4);
            result.PurchasePriceAfterVatInVnd = (int) (purchasePriceAfterVatInUsd * @params.UsdExchangeRate);

            result.VendorNetPriceInUsd = Math.Round(netPriceInUsd, 4);
            result.QuotationPriceInUsd = quotationPriceInUsd;

            return result;
        }
    }
}