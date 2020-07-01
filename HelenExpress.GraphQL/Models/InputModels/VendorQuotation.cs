#region

using System;
using GraphQL.Conventions;

#endregion

namespace HelenExpress.GraphQL.Models.InputModels
{
    [InputType]
    public class VendorQuotationUpdateInput
    {
        public Guid VendorId { get; set; }
        public VendorQuotationInput[] VendorQuotations { get; set; }
    }

    [InputType]
    public class VendorQuotationInput
    {
        public Guid Id { get; set; }
        public double? StartWeight { get; set; }
        public double EndWeight { get; set; }
        public VendorQuotationPriceInput[] ZonePrices { get; set; }
    }

    [InputType]
    public class VendorQuotationPriceInput
    {
        public Guid ZoneId { get; set; }
        public double PriceInUsd { get; set; }
    }
}