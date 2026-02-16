using Microsoft.AspNetCore.Http;
using Refahi.Notif.Application.Contract.Services;

namespace Refahi.Notif.Infrastructure.Persistence
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _context;

        public IdentityService(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public long UserId
        {
            get
            {
                return long.Parse(_context.HttpContext.User.FindFirst("sub").Value);
            }
        }

        //todo get from user
        public Guid DeviceId
        {
            get
            {
                return Guid.NewGuid();
            }
        }

        public string UserName
        {
            get
            {
                return _context.HttpContext.User.Identity.Name;
            }
        }
    }
}
