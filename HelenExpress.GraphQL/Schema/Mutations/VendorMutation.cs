#region

using System;
using System.Net.Http;
using System.Threading.Tasks;
using GraphQLDoorNet;
using GraphQLDoorNet.Abstracts;
using GraphQLDoorNet.Attributes;
using GraphQLDoorNet.Models;
using HelenExpress.Data.Entities;
using HelenExpress.Data.JSONModels;
using HelenExpress.GraphQL.Models.InputModels;
using Mapster;

#endregion

namespace HelenExpress.GraphQL.Schema.Mutations
{
    [ExtendMutation]
    public class VendorMutation : EntityMutationBase<Vendor, VendorInput>
    {
        public VendorMutation(IUnitOfWork unitOfWork, IInputMapper inputMapper) : base(unitOfWork, inputMapper)
        {
        }

        public async Task<MutationResult> UpdateQuotation(VendorQuotationUpdateInput input)
        {
            var vendorRepository = UnitOfWork.GetRepository<Vendor>();
            var vendor = await vendorRepository.FindAsync(input.VendorId);
            if (vendor != null)
            {
                vendor.VendorQuotations = input.VendorQuotations.Adapt<VendorQuotation[]>();
                vendor.LastUpdatedQuotation = DateTime.Now;
                await UnitOfWork.SaveChangesAsync();

                return new MutationResult {DidSuccess = true};
            }

            throw new HttpRequestException("Cannot find vendor for updating");
        }
    }
}