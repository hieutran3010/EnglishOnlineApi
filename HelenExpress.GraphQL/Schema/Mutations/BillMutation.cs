﻿#region

using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GraphQLDoorNet;
using GraphQLDoorNet.Abstracts;
using GraphQLDoorNet.Attributes;
using GraphQLDoorNet.Models;
using HelenExpress.Data;
using HelenExpress.Data.Entities;
using HelenExpress.GraphQL.Models.InputModels;
using HelenExpress.GraphQL.Services.Abstracts;
using HelenExpress.GraphQL.Services.Contracts;
using Mapster;
using Microsoft.EntityFrameworkCore;

#endregion

namespace HelenExpress.GraphQL.Schema.Mutations
{
    [ExtendMutation]
    public class BillMutation : EntityMutationBase<Bill, BillInput>
    {
        private readonly IUserProvider userProvider;
        public readonly IBillService billService;

        public BillMutation(IUnitOfWork unitOfWork, IUserProvider userProvider, IInputMapper inputMapper, IBillService billService) : base(unitOfWork, inputMapper)
        {
            this.userProvider = userProvider;
            this.billService = billService;
        }

        public override async Task<Bill> Add(BillInput input)
        {
            var role = this.userProvider.GetRole();
            if (role == Constants.UserRole.LICENSE) {
                input.Status = BillStatus.License;
            } else if (role == Constants.UserRole.ACCOUNTANT || role == Constants.UserRole.ADMIN) {
                input.Status = BillStatus.Accountant;
            }

            input.Period = input.Date.ToString("MM-yyyy");

            await CheckAndSaveBillDescription(input.Description);
            return await base.Add(input);
        }

        public override Task<Bill> Update(Guid id, BillInput input)
        {
            var role = this.userProvider.GetRole();
            if (string.IsNullOrWhiteSpace(role) || role == Constants.UserRole.SALE)
            {
                throw new UnauthorizedAccessException();
            }
            
            input.Period = input.Date.ToString("MM-yyyy");
            
            if (role == Constants.UserRole.LICENSE)
            {
                // a way to ignore unauthorized fields for license user
                var licenseBillInput = input.Adapt<LicenseBillInput>();
                input = licenseBillInput.Adapt<BillInput>();
            }
            
            return base.Update(id, input);
        }

        public async Task<MutationResult> AssignToAccountant(AssignToAccountantInput input) 
        {
            var billId = input.BillId;
            var billRepository = this.UnitOfWork.GetRepository<Bill>();
            var bill = await billRepository.GetQueryable().FirstOrDefaultAsync(bill => bill.Id == billId);
            if (bill == null) {
                throw new HttpRequestException($"Cannot find bill with bill with id = [{billId}]");
            }

            var vendorRepository = this.UnitOfWork.GetRepository<Vendor>();
            var vendor = await vendorRepository.GetQueryable().Where(v => v.Id == bill.VendorId)
                    .Include(v => v.Zones)
                    .FirstOrDefaultAsync();
            if (vendor != null) {
                bill.Status = BillStatus.Accountant;
                bill.VendorOtherFee = vendor.OtherFeeInUsd ?? 0;
                bill.VendorFuelChargePercent = vendor.FuelChargePercent ?? 0;

                var appParamsRepo = this.UnitOfWork.GetRepository<Params>();
                var usdExchangeRateParam = await appParamsRepo.GetQueryable().FirstOrDefaultAsync(ap => ap.Key == ParamsKey.VAT_PARAMS_KEY);
                if (usdExchangeRateParam != null)
                {
                    bill.UsdExchangeRate = Convert.ToInt32(usdExchangeRateParam.Value);
                }

                var purchasePrice = await this.billService.CountPurchasePriceAsync(this.UnitOfWork, new PurchasePriceCountingParams {
                    VendorId = bill.VendorId,
                    DestinationCountry = bill.DestinationCountry,
                    FuelChargePercent = bill.VendorFuelChargePercent,
                    OtherFeeInUsd = bill.VendorOtherFee,
                    Vat = bill.Vat,
                    UsdExchangeRate = bill.UsdExchangeRate ?? 0,
                    WeightInKg = bill.WeightInKg
                }, vendor);

                bill.QuotationPriceInUsd = purchasePrice.QuotationPriceInUsd;
                bill.VendorNetPriceInUsd = purchasePrice.VendorNetPriceInUsd;
                bill.VendorFuelChargeFeeInUsd = purchasePrice.FuelChargeFeeInUsd;
                bill.VendorFuelChargeFeeInVnd = purchasePrice.FuelChargeFeeInVnd;
                bill.PurchasePriceInUsd = purchasePrice.PurchasePriceInUsd;
                bill.PurchasePriceInVnd = purchasePrice.PurchasePriceInVnd;
                bill.ZoneName = purchasePrice.ZoneName;
                bill.PurchasePriceAfterVatInUsd = purchasePrice.PurchasePriceAfterVatInUsd;
                bill.PurchasePriceAfterVatInVnd = purchasePrice.PurchasePriceAfterVatInVnd;

                billRepository.Update(bill);
                await UnitOfWork.SaveChangesAsync();
            }

            return new MutationResult {DidSuccess = true};
        }

        public async Task<MutationResult> FinalBill(FinalBillInput input)
        {
            var billRepository = this.UnitOfWork.GetRepository<Bill>();
            var bill = await billRepository.GetQueryable().FirstOrDefaultAsync(b => b.Id == input.BillId);
            if (bill != null)
            {
                bill.Status = BillStatus.Done;
                bill.Profit = bill.SalePrice - bill.PurchasePriceAfterVatInVnd;
                bill.ProfitBeforeTax = bill.SalePrice - bill.PurchasePriceInVnd;
                billRepository.Update(bill);
                await this.UnitOfWork.SaveChangesAsync();
            }

            return new MutationResult {DidSuccess = true};
        }

        public async Task<MutationResult> ArchiveBill(ArchiveBillInput input)
        {
            var billRepository = this.UnitOfWork.GetRepository<Bill>();
            var bill = await billRepository.GetQueryable().FirstOrDefaultAsync(b=>b.Id == input.BillId);
            if (bill != null)
            {
                bill.IsArchived = true;
                billRepository.Update(bill);
                await this.UnitOfWork.SaveChangesAsync();
            }

            return new MutationResult {DidSuccess = true};
        }
        public async Task<MutationResult> CheckPrintedVatBill(Guid billId)  {
            var billRepository = this.UnitOfWork.GetRepository<Bill>();
            var bill = await billRepository.GetQueryable().FirstOrDefaultAsync(b => b.Id == billId);
            if (bill != null)
            {
                bill.IsPrintedVatBill = true;
                billRepository.Update(bill);
                await this.UnitOfWork.SaveChangesAsync();
            }

            return new MutationResult {DidSuccess = true};
        }

        private async Task CheckAndSaveBillDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description)) return;

            var billDescriptionRepository = UnitOfWork.GetRepository<BillDescription>();
            var existed = await billDescriptionRepository.GetQueryable()
                .FirstOrDefaultAsync(bd => bd.Name.ToLower() == description.ToLower());
            if (existed == null) await billDescriptionRepository.AddAsync(new BillDescription {Name = description});
        }
    }
}