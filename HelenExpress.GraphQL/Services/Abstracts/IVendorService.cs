using System;
using System.Threading.Tasks;
using GraphQLDoorNet.Abstracts;

namespace HelenExpress.GraphQL.Services.Abstracts
{
    public interface IVendorService
    {
        Task UpdateQuotationAfterDeletingZone(IUnitOfWork unitOfWork, Guid vendorId, Guid zoneId);
    }
}
