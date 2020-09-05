using System;
using GraphQL.Conventions;
using GraphQL.Conventions.Attributes.Execution.Wrappers;

namespace HelenExpress.GraphQL.Models.InputModels
{
    [InputType]
    public class ParcelServiceZoneInput: OptionalWrapper
    {
        public string Name { get; set; }

        public string[] Countries { get; set; }

        public Guid ParcelServiceId { get; set; }
    }
}