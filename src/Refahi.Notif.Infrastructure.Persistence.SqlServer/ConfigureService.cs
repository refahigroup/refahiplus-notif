using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refahi.Notif.Infrastructure.Persistence.Contract;
using Refahi.Notif.Infrastructure.Persistence.SqlServer.Context;

namespace Refahi.Notif.Infrastructure.Persistence.SqlServer
{
    public static class ConfigureService
    {
        public static void UseSqlServerAndHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("SqlServerNotif");

            services.AddDbContext<SqlNotifContext>(options =>
                    options.UseSqlServer(connectionString));

            services.AddHangfireServer();

            services.AddHangfire(x =>
                x.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                           .UseColouredConsoleLogProvider()
                           .UseSimpleAssemblyNameTypeSerializer()
                           .UseRecommendedSerializerSettings()

                           .UseSqlServerStorage(configuration.GetConnectionString("SqlServerNotifHangfire"), new SqlServerStorageOptions
                           {
                               CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                               SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                               QueuePollInterval = TimeSpan.Zero,
                               UseRecommendedIsolationLevel = true
                           })
            );

            services.AddScoped<IDbContext, SqlNotifContext>();
        }

        public static void UseSqlServerHangfire(this WebApplication app, IServiceProvider provider)
        {

        }

        public static void MigrateSqlServerDb(this WebApplication app, IServiceProvider provider)
        {
            var idbContext = provider.GetRequiredService<IDbContext>();
            var dbContext = idbContext as SqlNotifContext;

            dbContext?.Database.Migrate();
        }
    }
}
