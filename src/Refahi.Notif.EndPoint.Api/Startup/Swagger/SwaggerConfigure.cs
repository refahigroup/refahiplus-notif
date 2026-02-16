using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Refahi.Notif.EndPoint.Api.Startup.Swagger
{
    public static class SwaggerConfigure
    {
        public static void UseCustomSwagger(this WebApplication app, IServiceProvider provider)
        {
            app.UseSwagger()
                    .UseSwaggerUI(
                        c =>
                        {
                            // build a swagger endpoint for each discovered API version  
                            foreach (var description in provider.GetService<IApiVersionDescriptionProvider>().ApiVersionDescriptions)
                            {
                                c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                                description.GroupName.ToUpperInvariant());
                            }
                            c.OAuthClientId(app.Configuration["Identity:ClientId"]);
                            c.OAuthAppName(app.Configuration["Identity:OidcApiName"]);
                            c.OAuthUsePkce();
                        });
        }
    }
}
