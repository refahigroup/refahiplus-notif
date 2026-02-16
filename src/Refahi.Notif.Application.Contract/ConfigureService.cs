
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System.Reflection;

namespace Refahi.Notif.Application.Contract
{
    public static class ConfigureService
    {
        public static void AddApplicationContract(this IServiceCollection services)
        {


            var mapperConfig = new MapperConfiguration(
                mc =>
                {
                    mc.AddMaps(Assembly.GetExecutingAssembly());
                }, 
                NullLoggerFactory.Instance
            );

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

    }

}
