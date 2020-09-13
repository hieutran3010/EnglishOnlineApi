using GraphQL.Conventions;
using GraphQL.Conventions.Attributes.Execution.Wrappers;

namespace HelenExpress.GraphQL.Models.InputModels
{
    [InputType]
    public class ParcelServiceInput: OptionalWrapper
    {
        public string Name { get; set; }
    }
}