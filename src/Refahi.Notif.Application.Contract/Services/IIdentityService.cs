namespace Refahi.Notif.Application.Contract.Services
{
    public interface IIdentityService
    {
        long UserId { get; }
        Guid DeviceId { get; }
        string UserName { get; }
    }
}
