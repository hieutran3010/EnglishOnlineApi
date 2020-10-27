
using System;
using GraphQL.Conventions;
using HelenExpress.GraphQL.Models.InputModels;

namespace HelenExpress.GraphQL.Services.Contracts
{
    [InputType]
    public class PurchasePriceCountingParams
    {
        public Guid? VendorId { get; set; }
        public double WeightInKg { get; set; }
        public string DestinationCountry { get; set; }
        public double OtherFeeInUsd { get; set; }
        public double FuelChargePercent { get; set; }
        public double? Vat { get; set; }
        public double UsdExchangeRate { get; set; }
        public bool IsGetLatestQuotation { get; set; }
        public BillQuotationInput[] BillQuotations { get; set; }
        public string ServiceName { get; set; }
    }
}