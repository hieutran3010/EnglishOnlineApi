#region

using System;

#endregion

namespace HelenExpress.Data.JSONModels
{
    public class VendorQuotation
    {
        public Guid Id { get; set; }
        public double? StartWeight { get; set; }
        public double EndWeight { get; set; }
        public VendorQuotationPrice[] ZonePrices { get; set; }
    }

    public class VendorQuotationPrice
    {
        public Guid ZoneId { get; set; }
        public double? PriceInUsd { get; set; }
    }
}