using MassTransit;

namespace Refahi.Notif.Infrastructure.Consumer.MassTransit
{
    public class EnvironmentNameEndpointFormatter : SnakeCaseEndpointNameFormatter
    {
        private readonly string _environmentName;

        public EnvironmentNameEndpointFormatter(string environmentName)
        {
            _environmentName = environmentName;
        }

        public override string SanitizeName(string name)
        {
            name = $"{name}_{_environmentName}";

            return base.SanitizeName(name);
        }
    }
}
