namespace Refahi.Notif.Messages.Communication
{
    public class EndCall
    {
        public Guid CallId { get; set; }
        public long UserId { get; set; }
        public DateTime Time { get; set; }

    }



}
