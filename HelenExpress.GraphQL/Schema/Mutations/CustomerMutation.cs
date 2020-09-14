using System;
using System.Linq;
using System.Threading.Tasks;
using GraphQLDoorNet;
using GraphQLDoorNet.Abstracts;
using GraphQLDoorNet.Attributes;
using GraphQLDoorNet.Models;
using HelenExpress.Data.Entities;
using HelenExpress.GraphQL.Infrastructure.Extensions;
using HelenExpress.GraphQL.Models.InputModels;
using Microsoft.EntityFrameworkCore;

namespace HelenExpress.GraphQL.Schema.Mutations
{
    [ExtendMutation]
    public class CustomerMutation: EntityMutationBase<Customer, CustomerInput>
    {
        public CustomerMutation(IUnitOfWork unitOfWork, IInputMapper inputMapper) : base(unitOfWork, inputMapper)
        {
        }

        public override Task<Customer> Add(CustomerInput input)
        {
            input.NameNonUnicode = input.Name?.RemoveUnicode();
            return base.Add(input);
        }

        public override Task<Customer> Update(Guid id, CustomerInput input)
        {
            input.NameNonUnicode = input.Name?.RemoveUnicode();
            return base.Update(id, input);
        }

        public override async Task<HttpStatus> Delete(Guid id)
        {
            var billRepo = this.UnitOfWork.GetRepository<Bill>();
            var relatedBills = await billRepo.GetQueryable().Where(b =>
                b.SenderId != null && b.SenderId == id || b.ReceiverId != null && b.ReceiverId == id).ToListAsync();
            foreach (var bill in relatedBills)
            {
                if (bill.SenderId == id)
                {
                    bill.SenderId = null;
                }

                if (bill.ReceiverId == id)
                {
                    bill.ReceiverId = null;
                }
                
                billRepo.Update(bill);
            }
            return await base.Delete(id);
        }
    }
}