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
using HelenExpress.GraphQL.Models.InputModels;
using HelenExpress.GraphQL.Services.Abstracts;
using HelenExpress.GraphQL.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HelenExpress.GraphQL.Controllers
{
    [Authorize]
    [Route("export")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly IBackgroundTaskQueue<BillExportTaskContext> billExportTaskQueue;
        private readonly IUserProvider userProvider;
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileService fileService;
        private readonly IServiceScopeFactory serviceScopeFactory;

        private readonly IDictionary<string, ExcelFieldDefinition> billReportHeaderMappings =
            new Dictionary<string, ExcelFieldDefinition>
            {
                {"License", new ExcelFieldDefinition("Chứng Từ")},
                {"Accountant", new ExcelFieldDefinition("Kế Toán")},
                {"Sender", new ExcelFieldDefinition("Khách Gởi")},
                {"Receiver", new ExcelFieldDefinition("Khách Nhận")},
                {"Date", new ExcelFieldDefinition("Ngày")},
                {"AirlineBillId", new ExcelFieldDefinition("Bill Hãng Bay")},
                {"ChildBillId", new ExcelFieldDefinition("Bill Con")},
                {"VendorName", new ExcelFieldDefinition("Nhà Cung Cấp")},
                {"InternationalParcelVendor", new ExcelFieldDefinition("Dịch Vụ")},
                {"Description", new ExcelFieldDefinition("Thông Tin Hàng")},
                {"DestinationCountry", new ExcelFieldDefinition("Nước Đến")},
                {"WeightInKg", new ExcelFieldDefinition("Trọng Lượng(kg)") {FieldType = typeof(double)}},
                {"SalePrice", new ExcelFieldDefinition("Giá Bán(VNĐ)") {FieldType = typeof(double)}},
                {"CustomerPaymentType", new ExcelFieldDefinition("KHTT - Hình Thức")},
                {"CustomerPaymentAmount", new ExcelFieldDefinition("KHTT - Đã Trả") {FieldType = typeof(double)}},
                {"CustomerPaymentDebt", new ExcelFieldDefinition("KHTT - Còn Nợ") {FieldType = typeof(double)}},
                {"VendorNetPriceInUsd", new ExcelFieldDefinition("Giá Net(USD)") {FieldType = typeof(double)}},
                {"VendorOtherFee", new ExcelFieldDefinition("Phí Khác(USD)") {FieldType = typeof(double)}},
                {"VendorFuelChargePercent", new ExcelFieldDefinition("Phí Nhiên Liệu(%)") {FieldType = typeof(double)}},
                {"PurchasePriceInUsd", new ExcelFieldDefinition("Giá Mua(USD)") {FieldType = typeof(double)}},
                {"UsdExchangeRate", new ExcelFieldDefinition("Tỷ Giá") {FieldType = typeof(int)}},
                {
                    "PurchasePriceAfterVatInVnd",
                    new ExcelFieldDefinition("Giá Mua Sau Thuế(VNĐ)") {FieldType = typeof(int)}
                },
                {"VendorPaymentType", new ExcelFieldDefinition("TTNCC - Hình Thức")},
                {"VendorPaymentAmount", new ExcelFieldDefinition("TTNCC - Đã Trả") {FieldType = typeof(double)}},
                {"VendorPaymentDebt", new ExcelFieldDefinition("TTNCC - Còn Nợ") {FieldType = typeof(double)}}
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
                                    .Distinct().Where(u => !string.IsNullOrWhiteSpace(u));

                                var authService = scope.ServiceProvider.GetRequiredService<IAuth>();
                                var users = await authService.GetUsersByIds(userIds.ToArray());

                                var finalBills = bills.Select(bill =>
                                    new
                                    {
                                        Sale = users.FirstOrDefault(u => u.Id == bill.Sale)?.DisplayName,
                                        License = users.FirstOrDefault(u => u.Id == bill.License)?.DisplayName,
                                        Accountant = users.FirstOrDefault(u => u.Id == bill.Accountant)?.DisplayName,
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

                                var scopedFileService = scope.ServiceProvider.GetRequiredService<IFileService>();
                                var filePath = scopedFileService.SaveExcel(finalBills, billReportHeaderMappings,
                                    $"bill-export-{context.SessionId}.xlsx");

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