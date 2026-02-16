using Microsoft.AspNetCore.Mvc;
using Refahi.Notif.Infrastructure.Messaging.PushNotification.APN;

namespace Refahi.Notif.EndPoint.PushProxy.Controllers.APN
{
    [ApiController]
    [Route("[controller]")]
    public class APNController : Controller
    {
        private readonly APNNotificationSender _aPNNotificationSender;
        private readonly APNConfiguration _aPNConfiguration;

        public APNController(
            APNNotificationSender aPNNotificationSender,
            APNConfiguration aPNConfiguration)
        {
            _aPNNotificationSender = aPNNotificationSender;
            _aPNConfiguration = aPNConfiguration;
        }

        //todo authorize
        [HttpPost]
        public async Task<ActionResult> Send([FromBody] SendAPNDto request)
        {
            if (!Request.Headers.Any(x => x.Key == "secret" && x.Value == _aPNConfiguration.ProxySecret))
                return Unauthorized();

            return Ok(await _aPNNotificationSender.Send(request.Addresses[0], request.Title, request.Body, request.Url, request.Data, new Guid()));
        }

    }
}