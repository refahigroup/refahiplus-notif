using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Refahi.Notif.Infrastructure.Persistence.Contract;
using Refahi.Notif.Infrastructure.Persistence.Postgres.Context;

namespace Refahi.Notif.Infrastructure.Persistence.Postgres
{
    public static class ConfigureService
    {
        public static void UsePostreSqlAndHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("PostgresNotif")
                ?? throw new InvalidOperationException("Connection string 'PostgresNotif' not found.");

            string hanfireConnectionString = configuration.GetConnectionString("PostgresNotifHangfire")
                ?? throw new InvalidOperationException("Connection string 'PostgresNotifHangfire' not found.");


            connectionString = connectionString
                .Replace("{DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "")
                .Replace("{DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "");

            hanfireConnectionString = hanfireConnectionString
                .Replace("{DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "")
                .Replace("{DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "");


            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddDbContextPool<PgNotifContext>(opt => opt.UseNpgsql(connectionString));
            services.AddDbContextPool<PgHangfireContext>(opt => opt.UseNpgsql(hanfireConnectionString));

            services.AddScoped<IDbContext, PgNotifContext>();

            services.AddHangfire(x =>
                x.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseColouredConsoleLogProvider()
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()

                .UsePostgreSqlStorage(c =>
                    c.UseNpgsqlConnection(hanfireConnectionString), new PostgreSqlStorageOptions
                    {
                        PrepareSchemaIfNecessary = true
                    })
            );

            CreateHangfireDbIfNotExists(hanfireConnectionString);

            services.AddHangfireServer();
        }

        public static void UsePostgreHangfire(this WebApplication app, IServiceProvider provider)
        {
            //var hangfireDbContext = provider.GetRequiredService<PgHangfireContext>();
            //hangfireDbContext?.Database.EnsureCreated();
            //hangfireDbContext?.Database.Migrate();

            app.UseHangfireDashboard();
        }

        public static void MigratePostgreDb(this WebApplication app, IServiceProvider provider)
        {
            try
            {
                var idbContext = provider.GetRequiredService<IDbContext>();
                var dbContext = idbContext as PgNotifContext;

                dbContext?.Database.EnsureCreated();
                dbContext?.Database.Migrate();
            }
            catch(Exception ex)
            {

            }
        }

        private static void CreateHangfireDbIfNotExists(string connectionString)
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            var databaseName = builder.Database;

            builder.Database = "postgres";

            using var connection = new NpgsqlConnection(builder.ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT 1 FROM pg_database WHERE datname = '{databaseName}';";
            var exists = command.ExecuteScalar() != null;

            if (!exists)
            {
                command.CommandText = $"CREATE DATABASE \"{databaseName}\";";
                command.ExecuteNonQuery();
            }
        }
    }
}
