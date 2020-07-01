#region

using System.Linq;
using System.Security.Claims;
using EFPostgresEngagement.Abstract;
using HelenExpress.GraphQL.Services.Abstracts;
using Microsoft.AspNetCore.Http;

#endregion

namespace HelenExpress.GraphQL.Services
{
    public class UserProvider : IDbTracker, IUserProvider
    {
        private const string ROLE_NAME_KEY = "name";
        public string GetAuthor()
        {
            if (this.User == null)
            {
                return "N/A";
            }
            
            return this.User.Claims.Where(x => x.Type == ROLE_NAME_KEY).Select(x => x.Value).SingleOrDefault();
        }

        public ClaimsPrincipal User { get; private set; }
        
        public string GetRole()
        {
            return this.User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).SingleOrDefault();
        }

        public string GetUserId()
        {
            return this.User.Claims.Where(x => x.Type == "user_id").Select(x => x.Value).SingleOrDefault();
        }

        public UserProvider(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor?.HttpContext != null)
            {
                this.User = httpContextAccessor.HttpContext.User;
            }
        }
    }
}