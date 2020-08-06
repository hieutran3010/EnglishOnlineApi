#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthService.Firebase.Contracts;

#endregion

namespace AuthService.Firebase.Abstracts
{
    public interface IAuth
    {
        Task<User> RegisterAccountAsync(User record, Guid? tenantId = null);
        Task GrantRolesAsync(string userId, string[] roles, Guid? tenantId);
        Task<User> GetUserAsync(string uid);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByPhoneAsync(string phone);
        Task UpdateUserAsync(string uid, User user);

        /// <summary>
        ///     Get 1000 users
        /// </summary>
        /// <returns></returns>
        Task<List<User>> GetUsersAsync(Guid? tenantId = null);

        Task<List<User>> GetUsersByIds(string[] ids, Guid? tenantId = null);
    }
}