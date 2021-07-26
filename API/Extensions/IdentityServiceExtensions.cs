using API.Services;
using Domain;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using System.Text;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<AppUser>(opt =>
            {
                // We can configure auth opts here.
                opt.Password.RequireNonAlphanumeric = false;
            })
            // Add where we access objects from.
            .AddEntityFrameworkStores<DataContext>()
            // Add the sign in manager for the app user class.
            .AddSignInManager<SignInManager<AppUser>>();
            // Specify the key to decode and validate against.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    // How do we want api to validate that token is valid.
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            // Add the custom IsPostOwner security policy.
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("IsPostOwner", policy =>
                {
                    policy.Requirements.Add(new IsOwnerRequirement());
                });
            });
            // Add the handler.
            services.AddTransient<IAuthorizationHandler, IsOwnerRequirementHandler>();
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}
