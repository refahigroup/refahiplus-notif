namespace Refahi.Notif.Infrastructure.Messaging.Sms.KaveNegar
{
    public enum KaveSmsStatus
    {
        InQue = 1,
        Scheduled = 2,
        Sended = 3,
        Sended2 = 4,
        Failed = 6,
        Delivered = 10,
        UnDelivered = 11,
        Canceled = 13,
        Blocked = 14,
        UnValid = 100
    }

    public static class KaveSmsStatusExtensions
    {
        public static bool IsDelivered(this KaveSmsStatus status) => status == KaveSmsStatus.Delivered;
    }
}
