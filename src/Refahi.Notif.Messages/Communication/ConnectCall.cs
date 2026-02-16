namespace Refahi.Notif.Messages.Communication
{
    public class ConnectCall
    {
        public Guid CallId { get; set; }
        public long UserId { get; set; }
        public DateTime Time { get; set; }
        public string IpAddress { get; set; }
        public string CandidateType { get; set; }
        public string Protocol { get; set; }
    }


}
