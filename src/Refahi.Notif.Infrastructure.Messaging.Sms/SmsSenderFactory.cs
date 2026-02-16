using Microsoft.Extensions.Options;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Infrastructure.Messaging.Sms;

public class SmsSenderFactory : ISmsSenderFactory
{
    private readonly IEnumerable<ISmsSender> _services;
    private readonly MessageSenderProviderConfig _config;
    public SmsSenderFactory(IEnumerable<ISmsSender> services, IOptions<MessageSenderProviderConfig> configOptions)
    {
        _services = services;
        _config = configOptions.Value;
    }
    public ISmsSender GetService(SmsGateway? gateway)
    {
        return _services.FirstOrDefault(c => c.Gateway == (gateway ?? _config.Main))!;
    }

    public ISmsSender GetService(bool isAudio = false, bool useAlternative = false)
    {
        var gateway = isAudio ? _config.Audio : useAlternative ? _config.Alternative : _config.Main;

        return _services.FirstOrDefault(c => c.Gateway == gateway);
    }
    public ISmsSender GetServiceForVerify(bool isAudio = false, bool useAlternative = false)
    {
        var gateway = isAudio ? _config.AudioVerify : useAlternative ? _config.AlternativeVerify : _config.MainVerify;

        return _services.FirstOrDefault(c => c.Gateway == gateway);
    }
}