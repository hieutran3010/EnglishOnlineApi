using GraphQL.Conventions;

namespace HelenExpress.GraphQL.Models.InputModels
{
    [InputType]
    public class ParamsInput
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}