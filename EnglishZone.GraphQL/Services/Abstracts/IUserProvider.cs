using System.Security.Claims;

namespace EnglishZone.GraphQL.Services.Abstracts
{
    public interface IUserProvider
    {
        ClaimsPrincipal User { get; }
        string GetRole();
        string GetUserId();
    }
}