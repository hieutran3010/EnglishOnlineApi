namespace HelenExpress.GraphQL.Services.Abstracts
{
    using System.Threading.Tasks;
    using Contracts;
    using GraphQLDoorNet.Abstracts;
    using HelenExpress.Data.Entities;

    public interface IBillService
    {
        Task<PurchasePriceCountingResult> CountPurchasePriceAsync(IUnitOfWork unitOfWork,
            PurchasePriceCountingParams queryParams, Vendor vendor = null);

        public PurchasePriceCountingResult CountVendorNetPriceInUsd(Vendor vendor, PurchasePriceCountingParams @params,
            Zone zone, double? priceIncreasePercent = null);
    }
}