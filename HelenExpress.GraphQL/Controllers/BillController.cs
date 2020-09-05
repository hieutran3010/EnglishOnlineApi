namespace HelenExpress.GraphQL.Controllers
{
    using System;
    using System.Threading.Tasks;
    using GraphQLDoorNet.Abstracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Data.Entities;
    using Data.JSONModels;
    using Data;

    [Authorize]
    [Route("bill")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public BillController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPatch("updateDeliveryHistory/{billId}")]
        public async Task<IActionResult> UpdateDeliveryHistory(Guid billId, [FromBody] BillDeliveryHistory[] histories)
        {
            var billRepository = this.unitOfWork.GetRepository<Bill>();
            var bill = await billRepository.FindAsync(billId);
            if (bill != null)
            {
                bill.BillDeliveryHistories = histories;
                await this.unitOfWork.SaveChangesAsync();
            }

            return this.Ok();
        }
        
        [HttpPatch("archiveBill/{billId}")]
        public async Task<IActionResult> ArchiveBill(Guid billId)
        {
            var billRepository = this.unitOfWork.GetRepository<Bill>();
            var bill = await billRepository.FindAsync(billId);
            if (bill != null)
            {
                bill.IsArchived = true;
                await this.unitOfWork.SaveChangesAsync();
            }

            return this.Ok();
        }
        
        [HttpPatch("restoreArchivedBill/{billId}")]
        public async Task<IActionResult> RestoreArchivedBill(Guid billId)
        {
            var billRepository = this.unitOfWork.GetRepository<Bill>();
            var bill = await billRepository.FindAsync(billId);
            if (bill != null)
            {
                bill.IsArchived = false;
                await this.unitOfWork.SaveChangesAsync();
            }

            return this.Ok();
        }
        
        [HttpPatch("checkPrintedVatBill/{billId}")]
        public async Task<IActionResult> CheckPrintedVatBill(Guid billId)
        {
            var billRepository = this.unitOfWork.GetRepository<Bill>();
            var bill = await billRepository.FindAsync(billId);
            if (bill != null)
            {
                bill.IsPrintedVatBill = true;
                await this.unitOfWork.SaveChangesAsync();
            }

            return this.Ok();
        }
        
        [HttpPatch("returnFinalBillToAccountant/{billId}")]
        public async Task<IActionResult> ReturnFinalBillToAccountant(Guid billId)
        {
            var billRepository = this.unitOfWork.GetRepository<Bill>();
            var bill = await billRepository.FindAsync(billId);
            if (bill != null)
            {
                bill.Status = BillStatus.Accountant;
                await this.unitOfWork.SaveChangesAsync();
            }

            return this.Ok();
        }
    }
}