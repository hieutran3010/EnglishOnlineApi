#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AuthService.Firebase.Abstracts;
using AuthService.Firebase.Contracts;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

#endregion

namespace AuthService.Firebase.Firebase
{
    public class FirebaseAuthProvider : IAuth
    {
        private const string DEFAULT_ROLE_KEY = "roles";

        public FirebaseAuthProvider()
        {
            var googleCredentialsFilePath = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
            var fireBaseAppConfig = Environment.GetEnvironmentVariable("FIREBASE_CONFIG");
            if (!string.IsNullOrWhiteSpace(googleCredentialsFilePath))
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.GetApplicationDefault()
                });
            else if (!string.IsNullOrWhiteSpace(fireBaseAppConfig))
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromJson(fireBaseAppConfig)
                });
            else
                throw new ApplicationException("Cannot find config of Firebase");
        }

        public async Task<User> RegisterAccountAsync(User user, Guid? tenantId)
        {
            var args = new UserRecordArgs
            {
                Email = user.Email,
                EmailVerified = false,
                PhoneNumber = user.PhoneNumber,
                Password = user.Password,
                DisplayName = user.DisplayName,
                Disabled = false,
                PhotoUrl = user.AvatarUrl
            };
            var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(args);
            user.Id = userRecord.Uid;

            if (user.Roles != null && user.Roles.Any()) await SetUserRoles(userRecord, user.Roles, tenantId);

            return user;
        }

        public async Task GrantRolesAsync(string userId, string[] roles, Guid? tenantId)
        {
            var user = await FirebaseAuth.DefaultInstance.GetUserAsync(userId);
            if (user != null) await SetUserRoles(user, roles, tenantId);
        }

        public async Task<User> GetUserAsync(string uid)
        {
            var userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);

            return new User
            {
                Id = userRecord.Uid,
                Email = userRecord.Email,
                PhoneNumber = userRecord.PhoneNumber,
                DisplayName = userRecord.DisplayName,
                Disabled = userRecord.Disabled,
                Roles = GetUserRoles(userRecord.CustomClaims, null)
            };
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var userRecord = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);

            return new User
            {
                Id = userRecord.Uid,
                Email = userRecord.Email,
                PhoneNumber = userRecord.PhoneNumber,
                DisplayName = userRecord.DisplayName,
                Disabled = userRecord.Disabled,
                Roles = GetUserRoles(userRecord.CustomClaims, null)
            };
        }

        public async Task<User> GetUserByPhoneAsync(string phone)
        {
            var userRecord = await FirebaseAuth.DefaultInstance.GetUserByPhoneNumberAsync(phone);

            return new User
            {
                Id = userRecord.Uid,
                Email = userRecord.Email,
                PhoneNumber = userRecord.PhoneNumber,
                DisplayName = userRecord.DisplayName,
                Disabled = userRecord.Disabled,
                Roles = GetUserRoles(userRecord.CustomClaims, null)
            };
        }

        public async Task<List<User>> GetUsersAsync(Guid? tenantId)
        {
            var result = new List<User>();

            var pagedEnumerable = FirebaseAuth.DefaultInstance.ListUsersAsync(null);
            var responses = pagedEnumerable.AsRawResponses().GetEnumerator();

            while (await responses.MoveNext())
            {
                var response = responses.Current;
                foreach (var user in response.Users)
                {
                    var localUser = new User
                    {
                        Id = user.Uid,
                        Email = user.Email,
                        DisplayName = user.DisplayName,
                        PhoneNumber = user.PhoneNumber,
                        EmailVerified = user.EmailVerified,
                        AvatarUrl = user.PhotoUrl,
                        Disabled = user.Disabled
                    };

                    localUser.Roles = GetUserRoles(user.CustomClaims, tenantId);

                    result.Add(localUser);
                }
            }

            return result;
        }

        public async Task<List<User>> GetUsersByIds(string[] ids, Guid? tenantId = null)
        {
            var result = new List<User>();

            var identifiers = new List<UserIdentifier>();
            foreach (var uid in ids) identifiers.Add(new UidIdentifier(uid));

            var getUsersResult = await FirebaseAuth.DefaultInstance.GetUsersAsync(identifiers);
            var validUsers = getUsersResult.Users.Where(u => !u.Disabled && u.EmailVerified);

            foreach (var user in getUsersResult.Users)
                result.Add(new User
                {
                    AvatarUrl = user.PhotoUrl,
                    Disabled = user.Disabled,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    EmailVerified = user.EmailVerified,
                    PhoneNumber = user.PhoneNumber,
                    Id = user.Uid,
                    Roles = GetUserRoles(user.CustomClaims, tenantId)
                });

            return result;
        }

        private async Task SetUserRoles(UserRecord user, string[] roles, Guid? tenantId)
        {
            var roleKey = "";
            if (tenantId.HasValue)
                roleKey = tenantId.ToString();
            else
                roleKey = DEFAULT_ROLE_KEY;

            var claims = user.CustomClaims.Where(c => c.Key != roleKey)
                .ToDictionary(t => t.Key, t => t.Value);
            claims.Add(roleKey, string.Join('|', roles));

            await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(user.Uid, claims);
        }

        private string[] GetUserRoles(IReadOnlyDictionary<string, object> claims, Guid? tenantId)
        {
            if (claims != null && claims.Any())
            {
                var roleKey = tenantId.HasValue ? tenantId.ToString() : DEFAULT_ROLE_KEY;
                var roleClaim = claims.FirstOrDefault(k => k.Key.ToString() == roleKey).Value.ToString();
                return roleClaim.Split('|', StringSplitOptions.RemoveEmptyEntries);
            }

            return null;
        }

        public async Task UpdateUserAsync(string uid, User user)
        {
            var userRecord = await this.GetUserAsync(uid);
            if (user != null)
            {
                var updatedUserRecord = await FirebaseAuth.DefaultInstance.UpdateUserAsync(new UserRecordArgs
                {
                    Uid = uid,
                    Disabled = user.Disabled,
                    DisplayName = user.DisplayName,
                    PhoneNumber = user.PhoneNumber,
                    PhotoUrl = user.AvatarUrl,
                });

                var hasChangedRole = false;
                if (string.Join('|', userRecord.Roles) != string.Join('|', user.Roles))
                {
                    await this.SetUserRoles(updatedUserRecord, user.Roles, null);
                    hasChangedRole = true;
                }

                if (hasChangedRole || user.Disabled)
                {
                    await FirebaseAuth.DefaultInstance.RevokeRefreshTokensAsync(uid);
                }
            }
        }
    }
}