using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refahi.Notif.Domain.Contract;
using StackExchange.Redis;

namespace Refahi.Notif.Infrastructure.RedisCache
{
    public static class ConfigureService
    {
        public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var configurationOptions = new ConfigurationOptions();

            var addresses = configuration.GetSection("RedisConfig:Endpoints").Get<string[]>();
            var password = configuration.GetSection("RedisConfig:Password").Get<string>();
            var instanceName = configuration.GetSection("RedisConfig:InstanceName").Get<string>();

            foreach (var endpoint in addresses)
            {
                configurationOptions.EndPoints.Add(endpoint);

            }

            if (!string.IsNullOrEmpty(password))
                configurationOptions.Password = password;

            if(string.IsNullOrEmpty(instanceName))
                services.AddStackExchangeRedisCache(options =>
                {
                    options.ConfigurationOptions = configurationOptions;
                });
            else
                services.AddStackExchangeRedisCache(options =>
                {
                    options.ConfigurationOptions = configurationOptions;
                    options.InstanceName = instanceName;
                });

            services.AddScoped<ICacheService, RedisCacheService>();
        }
    }
}
