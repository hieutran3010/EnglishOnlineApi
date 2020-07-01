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
        public EntityQueryBase<Vendor> Vendor([Inject] EntityQueryBase<Vendor> entityQuery)
        {
            return entityQuery;
        }

        public EntityQueryBase<Zone> Zone([Inject] EntityQueryBase<Zone> entityQuery)
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
    }
}