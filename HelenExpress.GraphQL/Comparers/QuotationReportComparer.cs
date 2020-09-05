using System.Collections.Generic;
using System.Linq;
using HelenExpress.GraphQL.Models.ResponseModels;

namespace HelenExpress.GraphQL.Comparers
{
    public class QuotationReportComparer : IComparer<List<QuotationReportDetail>>
    {
        public int Compare(List<QuotationReportDetail> x, List<QuotationReportDetail> y)
        {
            var firtX = x.First().PurchasePriceInUsd;
            var firtY = y.First().PurchasePriceInUsd;

            if (firtX > firtY)
                return 1;
            if (firtX < firtY)
                return -1;
            return 0;
        }
    }
}