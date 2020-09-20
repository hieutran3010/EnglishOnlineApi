#region

using GraphQL.Conventions;
using GraphQLDoorNet;
using GraphQLDoorNet.Abstracts;
using HelenExpress.Data.Entities;
using HelenExpress.GraphQL.Models.InputModels;
using HelenExpress.GraphQL.Schema.Mutations;

#endregion

namespace HelenExpress.GraphQL.Schema
{
    internal sealed class Mutation : IMutation
    {
        public VendorMutation Vendor([Inject] VendorMutation entityMutation)
        {
            return entityMutation;
        }

        public ZoneMutation Zone([Inject] ZoneMutation entityMutation)
        {
            return entityMutation;
        }

        public CustomerMutation Customer([Inject] CustomerMutation entityMutation)
        {
            return entityMutation;
        }

        public BillMutation Bill([Inject] BillMutation entityMutation)
        {
            return entityMutation;
        }

        public EntityMutationBase<Params, ParamsInput> Params(
            [Inject] EntityMutationBase<Params, ParamsInput> entityMutation)
        {
            return entityMutation;
        }

        public ParcelServiceMutation ParcelService([Inject] ParcelServiceMutation entityMutation)
        {
            return entityMutation;
        }

        public ParcelServiceZoneMutation ParcelServiceZone(
            [Inject] ParcelServiceZoneMutation entityMutation)
        {
            return entityMutation;
        }

        public EntityMutationBase<ParcelServiceVendor, ParcelServiceVendorInput> ParcelServiceVendor(
            [Inject] EntityMutationBase<ParcelServiceVendor, ParcelServiceVendorInput> entityMutation)
        {
            return entityMutation;
        }

        public EntityMutationBase<SaleQuotationRate, SaleQuotationRateInput> SaleQuotationRate(
            [Inject] EntityMutationBase<SaleQuotationRate, SaleQuotationRateInput> entityMutation)
        {
            return entityMutation;
        }
    }
}