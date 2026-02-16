using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Refahi.Notif.EndPoint.SignalR
{
    public static class AuthenticationConfigureService
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // prevent from mapping "sub" claim to nameidentifier.
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
            Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var identityUrl = configuration.GetValue<string>("Identity:IdentityUrlExternal");
            var oidcApiName = configuration.GetValue<string>("Identity:OidcApiName");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.Authority = identityUrl;
                    options.RequireHttpsMetadata = false;
                    options.Audience = oidcApiName;
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = (_) =>
                        {
                            if (_.Request.Query["access_token"].Any())
                                _.Token = _.Request.Query["access_token"];
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = (_) =>
                        {
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = (_) =>
                        {
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
}
