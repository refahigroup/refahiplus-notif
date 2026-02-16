namespace Refahi.Notif.EndPoint.PushProxy.Controllers.Firebase
{
    public class SendFirebaseDto
    {

        public string[] Addresses { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Url { get; set; }
        public string Data { get; set; }
    }
}
