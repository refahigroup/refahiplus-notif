namespace Refahi.Notif.Messages.NotifCenter
{
    //todo add time to props & on update check that last update is not newer than this time
    public class SetUserPhoneNumber
    {
        public long UserId { get; set; }
        public string PhoneNumber { get; set; }

    }
}
