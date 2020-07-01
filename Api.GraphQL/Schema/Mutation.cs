using Api.Data.Entities;
using Api.GraphQL.InputModels;
using GraphQL.Conventions;
using GraphQLDoorNet;
using GraphQLDoorNet.Abstracts;

namespace Api.GraphQL.Schema
{
    internal sealed class Mutation : IMutation
    {
        public EntityMutationBase<SampleModel, SampleModelInput> Tenant(
            [Inject] EntityMutationBase<SampleModel, SampleModelInput> entityMutation) =>
            entityMutation;
    }
}