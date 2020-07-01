
using System;
using GraphQL.Conventions;

namespace HelenExpress.GraphQL.Services.Contracts
{
    [InputType]
    public class PurchasePriceCountingParams
    {
        public Guid VendorId { get; set; }
        public double WeightInKg { get; set; }
        public string DestinationCountry { get; set; }
        public double OtherFeeInUsd { get; set; }
        public double FuelChargePercent { get; set; }
        public double? Vat { get; set; }
        public double UsdExchangeRate { get; set; }
    }
}