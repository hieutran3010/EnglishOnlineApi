namespace HelenExpress.GraphQL.Controllers
{
    using System;
    using System.Threading.Tasks;
    using GraphQLDoorNet.Abstracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Data.Entities;
    using Data.JSONModels;

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
            var bill = await billRepository.GetQueryable().FirstOrDefaultAsync(b => b.Id == billId);
            if (bill != null)
            {
                bill.BillDeliveryHistories = histories;
                billRepository.Update(bill);
                await this.unitOfWork.SaveChangesAsync();
            }

            return this.Ok();
        }
    }
}