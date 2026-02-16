namespace Refahi.Notif.Infrastructure.Messaging.Sms.Nik
{
    public class NikSmsConfiguration
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Sender { get; set; }
        public string ApiUrl { get; set; }
        public string CreditUrl { get; set; }

        public string CheckUrl { get; set; }
        public int MinimumCredit { get; set; }

    }
}
