using System.Text.Json.Serialization;

namespace Refahi.Notif.Infrastructure.Messaging.Sms.KaveNegar
{
    public class SendSmsResultModel
    {
        public SendSmsReturnResultModel Return { get; set; }

    }
    public class SendSmsResultModel<T> : SendSmsResultModel
    {
        public T Entries { get; set; }
    }
    public class SendSmsReturnResultModel
    {
        public int Status { get; set; }
        public string StatusText { get; set; }
    }
    public class SendSmsEntriesResultModel
    {
        [JsonPropertyName("messageid")]
        public long Messageid { get; set; }
    }
    public class GetInfoResultModel
    {
        [JsonPropertyName("remaincredit")]
        public long RemainCredit { get; set; }
    }
}
