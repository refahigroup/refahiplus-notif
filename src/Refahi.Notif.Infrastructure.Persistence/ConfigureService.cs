using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refahi.Notif.Domain.Contract;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Infrastructure.Persistence.Repositories;

namespace Refahi.Notif.Infrastructure.Persistence;

public static class ConfigureService
{
    public static void AddPersistance(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISettingsHolder, SettingsHolder>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<INotificationEventRepository, NotificationEventRepository>();
        services.AddScoped<IVerifyMessageRepository, VerifyMessageRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFileService, FileService>();

        services.AddConfiguration<MinioConfiguration>(configuration, "MinioConfiguration");
    }

    private static void AddConfiguration<T>(this IServiceCollection services, IConfiguration configuration, string name) where T : class
    {
        if (configuration.GetChildren().Any(x => x.Key == name))
            services.AddSingleton(configuration.GetSection(name).Get<T>());
    }
}
