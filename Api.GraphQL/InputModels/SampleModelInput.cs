using Api.Data.Entities;
using GraphQL.Conventions.Attributes.Execution.Wrappers;
using GraphQLDoorNet.Attributes;

namespace Api.GraphQL.InputModels
{
    [InputType(EntityType = typeof(SampleModel))]
    public class SampleModelInput: OptionalWrapper
    {
        public string Name { get; set; }
    }
}