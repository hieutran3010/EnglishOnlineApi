#region

using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using GraphQLDoorNet;
using GraphQLDoorNet.Abstracts;
using GraphQLDoorNet.Attributes;
using HelenExpress.Data;
using HelenExpress.Data.Entities;
using HelenExpress.GraphQL.Models.ResponseModels;
using HelenExpress.GraphQL.Services.Abstracts;
using HelenExpress.GraphQL.Services.Contracts;
using Microsoft.EntityFrameworkCore;

#endregion

namespace HelenExpress.GraphQL.Schema.Queries
{
    [ExtendQuery]
    public class BillQuery : EntityQueryBase<Bill>
    {
        private readonly IBillService billService;

        public BillQuery(IUnitOfWork unitOfWork, IBillService billService) : base(unitOfWork)
        {
            this.billService = billService;
        }

        public Task<PurchasePriceCountingResult> CountPurchasePrice(PurchasePriceCountingParams queryParams)
        {
            return this.billService.CountPurchasePriceAsync(this.UnitOfWork, queryParams);
        }

        public async Task<List<VendorStatistic>> GetVendorStatistic(string query)
        {
            var billRepository = this.UnitOfWork.GetRepository<Bill>();
            var validBills = await billRepository.GetQueryable().Where(query).ToListAsync();
            var result = validBills.GroupBy(b => new {b.VendorId, b.VendorName}).Select(group => new VendorStatistic
            {
                VendorId = group.Key.VendorId,
                VendorName = group.Key.VendorName,
                TotalDebt = group.Sum(b => b.VendorPaymentDebt) ?? 0,
                TotalPayment = group.Sum(b => b.VendorPaymentAmount) ?? 0,
                TotalCashPayment = group.Where(b => b.VendorPaymentType == PaymentType.Cash)
                    .Sum(c => c.VendorPaymentAmount) ?? 0,
                TotalBankTransferPayment = group.Where(b => b.VendorPaymentType == PaymentType.BankTransfer)
                    .Sum(c => c.VendorPaymentAmount) ?? 0,
                TotalPurchase = group.Sum(b => b.PurchasePriceAfterVatInVnd) ?? 0,
                TotalSalePrice = group.Sum(b => b.SalePrice) ?? 0,
                TotalBill = group.Count(),
                TotalRawProfit = group.Sum(b => b.SalePrice - b.PurchasePriceAfterVatInVnd) ?? 0,
                TotalRawProfitBeforeTax = group.Sum(b => b.SalePrice - b.PurchasePriceInVnd) ?? 0,
                TotalProfit = group.Sum(b => b.Profit) ?? 0,
            }).ToList();

            return result;
        }

        public async Task<List<CustomerStatistic>> GetCustomerStatistic(string query)
        {
            var billRepository = this.UnitOfWork.GetRepository<Bill>();
            var validBills = await billRepository.GetQueryable().Where(query).ToListAsync();

            var result = validBills.GroupBy(b => new {b.SenderName, b.SenderPhone})
                .Select(group => new CustomerStatistic
                {
                    SenderName = group.Key.SenderName,
                    SenderPhone = group.Key.SenderPhone,
                    TotalDebt = group.Sum(b => b.CustomerPaymentDebt) ?? 0,
                    TotalPayment = group.Sum(b => b.CustomerPaymentAmount) ?? 0,
                    TotalSalePrice = group.Sum(b => b.SalePrice) ?? 0,
                    TotalPurchase = group.Sum(b => b.PurchasePriceAfterVatInVnd) ?? 0,
                    TotalBill = group.Count(),
                    TotalCashPayment = group.Where(b => b.VendorPaymentType == PaymentType.Cash)
                        .Sum(c => c.VendorPaymentAmount) ?? 0,
                    TotalBankTransferPayment = group.Where(b => b.VendorPaymentType == PaymentType.BankTransfer)
                        .Sum(c => c.VendorPaymentAmount) ?? 0,
                    TotalRawProfit = group.Sum(b => b.SalePrice - b.PurchasePriceAfterVatInVnd) ?? 0,
                    TotalRawProfitBeforeTax = group.Sum(b => b.SalePrice - b.PurchasePriceInVnd) ?? 0,
                    TotalProfit = group.Sum(b => b.Profit) ?? 0,
                }).ToList();

            return result;
        }
    }
}