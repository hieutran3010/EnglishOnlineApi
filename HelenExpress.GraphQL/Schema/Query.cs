#region

using GraphQL.Conventions;
using GraphQLDoorNet;
using GraphQLDoorNet.Abstracts;
using HelenExpress.Data.Entities;
using HelenExpress.GraphQL.Schema.Queries;

#endregion

namespace HelenExpress.GraphQL.Schema
{
    internal sealed class Query : IQuery
    {
        public VendorQuery Vendor([Inject] VendorQuery entityQuery)
        {
            return entityQuery;
        }

        public ZoneQuery Zone([Inject] ZoneQuery entityQuery)
        {
            return entityQuery;
        }

        public EntityQueryBase<Customer> Customer([Inject] EntityQueryBase<Customer> entityQuery)
        {
            return entityQuery;
        }

        public BillQuery Bill([Inject] BillQuery entityQuery)
        {
            return entityQuery;
        }

        public EntityQueryBase<BillDescription> BillDescription([Inject] EntityQueryBase<BillDescription> entityQuery)
        {
            return entityQuery;
        }

        public EntityQueryBase<ExportSession> ExportSession([Inject] EntityQueryBase<ExportSession> entityQuery)
        {
            return entityQuery;
        }

        public EntityQueryBase<Params> Params([Inject] EntityQueryBase<Params> entityQuery)
        {
            return entityQuery;
        }

        public EntityQueryBase<ParcelService> ParcelService([Inject] EntityQueryBase<ParcelService> entityQuery)
        {
            return entityQuery;
        }

        public EntityQueryBase<ParcelServiceZone> ParcelServiceZone(
            [Inject] EntityQueryBase<ParcelServiceZone> entityQuery)
        {
            return entityQuery;
        }

        public EntityQueryBase<ParcelServiceVendor> ParcelServiceVendor(
            [Inject] EntityQueryBase<ParcelServiceVendor> entityQuery)
        {
            return entityQuery;
        }
        
        public EntityQueryBase<SaleQuotationRate> SaleQuotationRate(
            [Inject] EntityQueryBase<SaleQuotationRate> entityQuery)
        {
            return entityQuery;
        }
    }
}