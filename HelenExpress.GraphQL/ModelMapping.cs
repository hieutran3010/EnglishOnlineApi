using HelenExpress.Data.Entities;
using HelenExpress.Data.JSONModels;
using HelenExpress.GraphQL.Models.InputModels;
using Mapster;

namespace HelenExpress.GraphQL
{
    public class ModelMapping
    {
        public static void Mapping()
        {
            var config = new TypeAdapterConfig();
            config.ForType<BillInput, Bill>();
            config.ForType<CustomerInput, Customer>();
            config.ForType<VendorInput, Vendor>();
            config.ForType<VendorQuotationInput, VendorQuotation>();
            config.ForType<VendorQuotationPriceInput, VendorQuotationPrice>();
            config.ForType<ZoneInput, Zone>();
            config.ForType<ParamsInput, Params>();

            config.ForType<BillInput, LicenseBillInput>().TwoWays();
        }
    }
}