using System;
using GraphQL.Conventions;
using GraphQL.Conventions.Attributes.Execution.Wrappers;

namespace HelenExpress.GraphQL.Models.InputModels
{
    [InputType]
    public class ParcelServiceVendorInput: OptionalWrapper
    {
        public Guid ParcelServiceId { get; set; }
        public Guid VendorId { get; set; }
    }
}
