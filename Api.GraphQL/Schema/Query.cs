using Api.Data.Entities;
using GraphQL.Conventions;
using GraphQLDoorNet;
using GraphQLDoorNet.Abstracts;

namespace Api.GraphQL.Schema
{
    internal sealed class Query: IQuery
    {
        public EntityQueryBase<SampleModel> SampleModel([Inject] EntityQueryBase<SampleModel> entityQuery) => entityQuery;
    }
}