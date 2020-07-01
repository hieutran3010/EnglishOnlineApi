using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AuthService.Firebase.Abstracts;
using BackgroundTaskQueueNet.Abstracts;
using GraphQLDoorNet.Abstracts;
using HelenExpress.Data;
using HelenExpress.Data.Entities;
using HelenExpress.GraphQL.HostedServices.ExportBill;
using HelenExpress.GraphQL.Infrastructure.Extensions;
using HelenExpress.GraphQL.Models.InputModels;
using HelenExpress.GraphQL.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HelenExpress.GraphQL.Controllers
{
    [Authorize]
    [Route("api/export")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly IBackgroundTaskQueue<BillExportTaskContext> billExportTaskQueue;
        private readonly IUserProvider userProvider;
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileService fileService;
        private readonly IServiceScopeFactory serviceScopeFactory;

        private IDictionary<string, string> BillReportHeaderMappings = new Dictionary<string, string> 
        {
            { "License", "Chứng Từ" },
            { "Accountant", "Kế Toán" },
            { "Sender", "Khách Gởi" },
            { "Receiver", "Khách Nhận" },
            { "Date", "Ngày" },
            { "ChildBillId", "Bill Con" },
            { "AirlineBillId", "Bill Hãng Bay" },
            { "VendorName", "Nhà Cung Cấp" },
            { "InternationalParcelVendor", "Dịch Vụ" },
            { "Description", "Thông Tin Hàng" },
            { "DestinationCountry", "Nước Đến" },
            { "WeightInKg", "Trọng Lượng(kg)" },
            { "SalePrice", "Giá Bán(VNĐ)" },
            { "CustomerPaymentType", "KHTT - Hình Thức" },
            { "CustomerPaymentAmount", "KHTT - Đã Trả" },
            { "CustomerPaymentDebt", "KHTT - Còn Nợ" },
            { "VendorNetPriceInUsd", "Giá Net(USD)" },
            { "VendorOtherFee", "Phí Khác(USD)" },
            { "VendorFuelChargePercent", "Phí Nhiên Liệu(%)" },
            { "PurchasePriceInUsd", "Giá Mua(USD)" },
            { "UsdExchangeRate", "Tỷ Giá" },
            { "PurchasePriceAfterVatInVnd", "Giá Mua Sau Thuế(VNĐ)" },
            { "VendorPaymentType", "TTNCC - Hình Thức"},
            { "VendorPaymentAmount", "TTNCC - Đã Trả"},
            { "VendorPaymentDebt", "TTNCC - Còn Nợ"}
        };

        public ExportController(IBackgroundTaskQueue<BillExportTaskContext> billExportTaskQueue,
            IUserProvider userProvider, IUnitOfWork unitOfWork, IFileService fileService,
            IServiceScopeFactory serviceScopeFactory)
        {
            this.billExportTaskQueue = billExportTaskQueue;
            this.userProvider = userProvider;
            this.unitOfWork = unitOfWork;
            this.fileService = fileService;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        [HttpPost("requestExportBillReport")]
        public async Task<IActionResult> RequestExportBillReport([FromBody] BillReportExportRequestParams requestParams)
        {
            var query = requestParams.Query;
            await this.RemoveExistedSession(ExportType.BILL_REPORT);

            if (!string.IsNullOrWhiteSpace(query))
            {
                var sessionId = DateTime.Now.Ticks.ToString();
                this.billExportTaskQueue.QueueBackgroundWorkItem(new BillExportTaskContext
                {
                    SessionId = sessionId,
                    UserId = this.userProvider.GetUserId(),
                    Query = query,
                    Note = requestParams.Note,
                    WorkItem = async (cancellationToken, context) =>
                    {
                        using var scope = this.serviceScopeFactory.CreateScope();
                        var billExportContext = (BillExportTaskContext) context;
                        var scopedUnitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        var billExportSessionRepository = scopedUnitOfWork.GetRepository<ExportSession>();
                        var newSession = new ExportSession
                        {
                            UserId = billExportContext.UserId,
                            Status = ExportSessionStatus.WORKING,
                            ExportType = ExportType.BILL_REPORT,
                            Note = billExportContext.Note,
                        };
                        await billExportSessionRepository.AddAsync(newSession, cancellationToken);
                        await scopedUnitOfWork.SaveChangesAsync(cancellationToken);

                        try
                        {
                            var billRepo = scopedUnitOfWork.GetRepository<Bill>();
                            var bills = await billRepo.GetQueryable().Where(billExportContext.Query)
                                .OrderByDescending(b => b.Date)
                                .Select(b => new
                                {
                                    Sale = b.SaleUserId,
                                    License = b.LicenseUserId,
                                    Accountant = b.AccountantUserId,
                                    Sender = $"{b.SenderName}-{b.SenderPhone}-{b.SenderAddress}",
                                    Receiver = $"{b.ReceiverName}-{b.ReceiverPhone}-{b.ReceiverAddress}",
                                    b.Date,
                                    b.ChildBillId,
                                    b.AirlineBillId,
                                    b.VendorName,
                                    b.InternationalParcelVendor,
                                    b.Description,
                                    b.DestinationCountry,
                                    b.WeightInKg,
                                    b.SalePrice,
                                    b.CustomerPaymentType,
                                    b.CustomerPaymentAmount,
                                    b.CustomerPaymentDebt,
                                    b.VendorNetPriceInUsd,
                                    b.VendorOtherFee,
                                    b.VendorFuelChargePercent,
                                    b.PurchasePriceInUsd,
                                    b.Vat,
                                    b.UsdExchangeRate,
                                    b.PurchasePriceAfterVatInVnd,
                                    b.VendorPaymentType,
                                    b.VendorPaymentAmount,
                                    b.VendorPaymentDebt
                                }).ToListAsync(cancellationToken);

                            if (bills.Any())
                            {
                                var userIds = bills.SelectMany(b => new List<string> {b.Sale, b.Accountant, b.License})
                                    .Distinct();
                                var authService = scope.ServiceProvider.GetRequiredService<IAuth>();
                                var users = await authService.GetUsersByIds(userIds.ToArray());

                                var finalBills = (from bill in bills
                                    join saleUser in users on bill.Sale equals saleUser.Id
                                    join licenseUser in users on bill.License equals licenseUser.Id
                                    join accountantUser in users on bill.Accountant equals accountantUser.Id
                                    select new
                                    {
                                        Sale = saleUser.DisplayName,
                                        License = licenseUser.DisplayName,
                                        Accountant = accountantUser.DisplayName,
                                        bill.Sender,
                                        bill.Receiver,
                                        bill.Date,
                                        bill.ChildBillId,
                                        bill.AirlineBillId,
                                        bill.VendorName,
                                        bill.InternationalParcelVendor,
                                        bill.Description,
                                        bill.DestinationCountry,
                                        bill.WeightInKg,
                                        bill.SalePrice,
                                        bill.CustomerPaymentType,
                                        bill.CustomerPaymentAmount,
                                        bill.CustomerPaymentDebt,
                                        bill.VendorNetPriceInUsd,
                                        bill.VendorOtherFee,
                                        bill.VendorFuelChargePercent,
                                        bill.PurchasePriceInUsd,
                                        bill.Vat,
                                        bill.UsdExchangeRate,
                                        bill.PurchasePriceAfterVatInVnd,
                                        bill.VendorPaymentType,
                                        bill.VendorPaymentAmount,
                                        bill.VendorPaymentDebt
                                    }).ToList();

                                var csv = finalBills.ToCsv(BillReportHeaderMappings);
                                var scopedFileService = scope.ServiceProvider.GetRequiredService<IFileService>();
                                var filePath = scopedFileService.Save(csv, $"bill-export-{context.SessionId}.csv");

                                newSession.Status = ExportSessionStatus.DONE;
                                newSession.FilePath = filePath;
                                billExportSessionRepository.Update(newSession);
                                await scopedUnitOfWork.SaveChangesAsync(cancellationToken);
                            }
                            else
                            {
                                billExportSessionRepository.Remove(newSession);
                                await scopedUnitOfWork.SaveChangesAsync(cancellationToken);
                            }
                        }
                        catch (Exception)
                        {
                            billExportSessionRepository.Remove(newSession);
                            await scopedUnitOfWork.SaveChangesAsync(cancellationToken);
                            throw;
                        }
                    }
                });

                return this.Accepted(sessionId);
            }

            return this.Ok();
        }

        [HttpGet("downloadBillReport")]
        public async Task<IActionResult> DownloadBillReport()
        {
            var userId = this.userProvider.GetUserId();
            var exportSessionRepo = this.unitOfWork.GetRepository<ExportSession>();
            var billExportSession = await exportSessionRepo.GetQueryable().FirstOrDefaultAsync(es =>
                es.UserId == userId && es.ExportType == ExportType.BILL_REPORT &&
                es.Status == ExportSessionStatus.DONE);
            if (billExportSession != null)
            {
                var result = this.fileService.FetchFile(billExportSession.FilePath);
                if (result.archiveData != null)
                {
                    await this.RemoveExistedSession(ExportType.BILL_REPORT);
                    return this.File(result.archiveData, result.fileType, result.archiveName);
                }
            }

            return this.NotFound();
        }

        private async Task RemoveExistedSession(string exportType)
        {
            var userId = this.userProvider.GetUserId();
            var exportSessionRepo = this.unitOfWork.GetRepository<ExportSession>();
            var existedExport = await exportSessionRepo.GetQueryable()
                .FirstOrDefaultAsync(es => es.UserId == userId && es.ExportType == exportType);
            if (existedExport != null)
            {
                exportSessionRepo.Remove(existedExport);
                await unitOfWork.SaveChangesAsync();
                if (!string.IsNullOrWhiteSpace(existedExport.FilePath) && System.IO.File.Exists(existedExport.FilePath))
                {
                    System.IO.File.Delete(existedExport.FilePath);
                }
            }
        }
    }
}