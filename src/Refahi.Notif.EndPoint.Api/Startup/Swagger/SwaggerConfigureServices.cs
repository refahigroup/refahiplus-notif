using Microsoft.OpenApi.Models;
using Refahi.Notif.EndPoint.Api.Filters;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

namespace Refahi.Notif.EndPoint.Api.Startup.Swagger
{
    public static class SwaggerConfigureServices
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {

                var oidcApiName = configuration.GetValue<string>("Identity:OidcApiName");
                var clientId = configuration.GetValue<string>("Identity:ClientId");
                options.OperationFilter<SwaggerDefaultValues>();
                //options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                //{
                //    Type = SecuritySchemeType.OAuth2,
                //    Flows = new OpenApiOAuthFlows()
                //    {
                //        AuthorizationCode = new OpenApiOAuthFlow()
                //        {
                //            AuthorizationUrl = new Uri($"{configuration.GetValue<string>("Identity:IdentityUrlExternal")}/connect/authorize"),
                //            TokenUrl = new Uri($"{configuration.GetValue<string>("Identity:IdentityUrlExternal")}/connect/token"),
                //            Scopes = new Dictionary<string, string>()
                //            {
                //                { oidcApiName, clientId }
                //            }
                //        }
                //    }
                //});

                options.OperationFilter<AuthorizeCheckOperationFilter>();
                options.AddEnumsWithValuesFixFilters();

            });

            return services;
        }

    }



}
