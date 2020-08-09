#region

using System;
using EFPostgresEngagement.DataAnnotationAttributes;

#endregion

namespace HelenExpress.Data.Entities
{
    public class Bill : EntityBase
    {
        public string SaleUserId { get; set; }
        public string LicenseUserId { get; set; }
        public string AccountantUserId { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string SenderAddress { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceiverAddress { get; set; }
        [SimpleIndex] public DateTime Date { get; set; }

        [SimpleIndex] public string Period { get; set; }

        [UniqueIndex] public string ChildBillId { get; set; }

        [UniqueIndex] public string AirlineBillId { get; set; }

        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public Vendor Vendor { get; set; }
        public Guid? SenderId { get; set; }
        public Customer Sender { get; set; }
        public Guid? ReceiverId { get; set; }
        public Customer Receiver { get; set; }
        public string InternationalParcelVendor { get; set; }
        public string Description { get; set; }
        public string DestinationCountry { get; set; }
        public double WeightInKg { get; set; }
        public double? SalePrice { get; set; }
        public double? PurchasePriceInUsd { get; set; }
        public int? PurchasePriceInVnd { get; set; }
        public double? PurchasePriceAfterVatInUsd { get; set; }
        public int? PurchasePriceAfterVatInVnd { get; set; }
        public double? Profit { get; set; }
        public double? ProfitBeforeTax { get; set; }
        public uint? Vat { get; set; }
        public string Status { get; set; }
        public double? VendorNetPriceInUsd { get; set; }
        public double VendorOtherFee { get; set; }
        public double VendorFuelChargePercent { get; set; }
        public double? VendorFuelChargeFeeInUsd { get; set; }
        public double? VendorFuelChargeFeeInVnd { get; set; }
        public string CustomerPaymentType { get; set; }
        public double? CustomerPaymentAmount { get; set; }
        public double? CustomerPaymentDebt { get; set; }
        public string VendorPaymentType { get; set; }
        public double? VendorPaymentAmount { get; set; }
        public double? VendorPaymentDebt { get; set; }
        public bool IsArchived { get; set; }
        public int? UsdExchangeRate { get; set; }
        public double? QuotationPriceInUsd { get; set; }
        public string ZoneName { get; set; }
        public bool IsPrintedVatBill { get; set; }
    }
}