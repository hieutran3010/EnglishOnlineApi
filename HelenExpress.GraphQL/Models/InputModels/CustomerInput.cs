#region

using GraphQL.Conventions;
using GraphQL.Conventions.Attributes.Execution.Wrappers;

#endregion

namespace HelenExpress.GraphQL.Models.InputModels
{
    [InputType]
    public class CustomerInput : OptionalWrapper
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Hint { get; set; }
        public string NameNonUnicode { get; set; }
    }
}