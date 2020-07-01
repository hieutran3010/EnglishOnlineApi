#region

using GraphQL.Conventions;
using GraphQL.Conventions.Attributes.Execution.Wrappers;

#endregion

namespace HelenExpress.GraphQL.Models.InputModels
{
    [InputType]
    public class VendorInput : OptionalWrapper
    {
        public string Name { get; set; }
        public string OfficeAddress { get; set; }
        public string Phone { get; set; }
        public double? OtherFeeInUsd { get; set; }
        public double? FuelChargePercent { get; set; }
        public bool IsStopped { get; set; } = false;
    }
}