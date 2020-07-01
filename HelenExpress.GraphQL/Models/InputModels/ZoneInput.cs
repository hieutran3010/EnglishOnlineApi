#region

using System;
using GraphQL.Conventions;
using GraphQL.Conventions.Attributes.Execution.Wrappers;

#endregion

namespace HelenExpress.GraphQL.Models.InputModels
{
    [InputType]
    public class ZoneInput : OptionalWrapper
    {
        public string Name { get; set; }
        public string[] Countries { get; set; }
        public Guid VendorId { get; set; }
    }
}