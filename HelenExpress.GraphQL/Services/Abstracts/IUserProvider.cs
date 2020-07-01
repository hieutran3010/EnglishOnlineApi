using System.Security.Claims;

namespace HelenExpress.GraphQL.Services.Abstracts
{
    public interface IUserProvider
    {
        ClaimsPrincipal User { get; }
        string GetRole();
        string GetUserId();
    }
}