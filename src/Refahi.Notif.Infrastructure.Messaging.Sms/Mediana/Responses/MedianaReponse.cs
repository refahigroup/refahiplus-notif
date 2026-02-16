namespace Refahi.Notif.Infrastructure.Messaging.Sms.Mediana.Responses
{
    public class MedianaReponse<T>
    {
        public T Data { get; set; }

        public string Status { get; set; }

        public int Code { get; set; }

        public string ErrorMessage { get; set; }
    }
}
