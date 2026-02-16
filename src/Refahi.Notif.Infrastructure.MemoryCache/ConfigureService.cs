using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refahi.Notif.Domain.Contract;

namespace Refahi.Notif.Infrastructure.MemoryCache
{
    public static class ConfigureService
    {
        public static void AddInMemoryCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddScoped<ICacheService, InMemoryCacheService>();
        }
    }
}
