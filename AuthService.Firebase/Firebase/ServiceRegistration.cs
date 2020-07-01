#region

using AuthService.Firebase.Abstracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace AuthService.Firebase.Firebase
{
    public static class ServiceRegistration
    {
        public static void AddFirebaseAuthService(this IServiceCollection services, string projectId)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var issuer = $"https://securetoken.google.com/{projectId}";

                    options.Authority = issuer;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = projectId,
                        ValidateLifetime = true
                    };
                });

            services.AddSingleton<IAuth, FirebaseAuthProvider>();
        }
    }
}