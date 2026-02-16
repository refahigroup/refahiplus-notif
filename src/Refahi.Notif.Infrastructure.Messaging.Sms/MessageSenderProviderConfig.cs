using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Infrastructure.Messaging.Sms;

public class MessageSenderProviderConfig
{
    public SmsGateway Main { get; set; }
    public SmsGateway Alternative { get; set; }
    public SmsGateway Audio { get; set; }
    public SmsGateway MainVerify { get; set; }
    public SmsGateway AlternativeVerify { get; set; }
    public SmsGateway AudioVerify { get; set; }
}