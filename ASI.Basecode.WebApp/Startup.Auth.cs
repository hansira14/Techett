using ASI.Basecode.WebApp.Authentication;
using ASI.Basecode.WebApp.Extensions.Configuration;
using ASI.Basecode.Resources.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ASI.Basecode.WebApp
{
    // Authorization configuration
    internal partial class StartupConfigurer
    {
        private readonly SymmetricSecurityKey _signingKey;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly TokenProviderOptions _tokenProviderOptions;

        /// <summary>
        /// Configure authorization
        /// </summary>
        private void ConfigureAuthorization()
        {
            var token = Configuration.GetTokenAuthentication();
            var tokenProviderOptionsFactory = this._services.BuildServiceProvider().GetService<TokenProviderOptionsFactory>();
            var tokenValidationParametersFactory = this._services.BuildServiceProvider().GetService<TokenValidationParametersFactory>();
            var tokenValidationParameters = tokenValidationParametersFactory.Create();

            this._services.AddAuthentication(Const.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
            })
            .AddCookie(Const.AuthenticationScheme, options =>
            {
                options.Cookie = new CookieBuilder()
                {
                    IsEssential = true,
                    SameSite = SameSiteMode.Lax,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest,
                    Name = $"{this._environment.ApplicationName}_{token.CookieName}"
                };
                options.LoginPath = new PathString("/Account/Login");
                options.AccessDeniedPath = new PathString("/Home/Forbidden");
                options.ReturnUrlParameter = "ReturnUrl";
                options.TicketDataFormat = new CustomJwtDataFormat(SecurityAlgorithms.HmacSha256, _tokenValidationParameters, Configuration, tokenProviderOptionsFactory);
            });

            this._services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAuthenticatedUser", policy =>
                {
                    policy.RequireAuthenticatedUser();
                });

                options.AddPolicy("RequireSuperAdminRole", policy =>
                    policy.RequireAssertion(context => 
                        context.User.HasClaim(c => c.Type == ClaimTypes.Role && 
                            c.Value.Equals(Roles.SuperAdmin, StringComparison.OrdinalIgnoreCase))));

                options.AddPolicy("RequireAdminRole", policy =>
                    policy.RequireAssertion(context => 
                        context.User.HasClaim(c => c.Type == ClaimTypes.Role && 
                            (c.Value.Equals(Roles.Admin, StringComparison.OrdinalIgnoreCase) || 
                             c.Value.Equals(Roles.SuperAdmin, StringComparison.OrdinalIgnoreCase)))));

                options.AddPolicy("RequireAgentRole", policy =>
                    policy.RequireAssertion(context => 
                        context.User.HasClaim(c => c.Type == ClaimTypes.Role && 
                            (c.Value.Equals(Roles.Agent, StringComparison.OrdinalIgnoreCase) || 
                             c.Value.Equals(Roles.Admin, StringComparison.OrdinalIgnoreCase) || 
                             c.Value.Equals(Roles.SuperAdmin, StringComparison.OrdinalIgnoreCase)))));

                options.AddPolicy("RequireUserRole", policy =>
                    policy.RequireAssertion(context => 
                        context.User.HasClaim(c => c.Type == ClaimTypes.Role && 
                            c.Value.Equals(Roles.User, StringComparison.OrdinalIgnoreCase))));
            });

            this._services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter("RequireAuthenticatedUser"));
            });

            this._services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Home/Forbidden";
                options.Cookie.Name = "YourAppCookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Account/Login";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });
        }
    }
}
