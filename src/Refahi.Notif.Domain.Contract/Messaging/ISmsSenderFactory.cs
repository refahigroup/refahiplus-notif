using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Domain.Contract.Messaging;

public interface ISmsSenderFactory
{
    ISmsSender GetService(SmsGateway? Gateway);
    ISmsSender GetService(bool isAudio = false, bool useAlternative = false);
    ISmsSender GetServiceForVerify(bool isAudio = false, bool useAlternative = false);
}