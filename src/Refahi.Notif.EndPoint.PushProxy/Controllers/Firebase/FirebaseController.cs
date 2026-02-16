using Microsoft.AspNetCore.Mvc;
using Refahi.Notif.Infrastructure.Messaging.PushNotification.Firebase;

namespace Refahi.Notif.EndPoint.PushProxy.Controllers.Firebase
{
    [ApiController]
    [Route("[controller]")]
    public class FirebaseController : Controller
    {
        private readonly FirebaseNotificationSender _fireNotificationSender;
        private readonly FirebaseConfiguration _firebaseConfiguration;
        public FirebaseController(FirebaseNotificationSender fireNotificationSender, FirebaseConfiguration firebaseConfiguration)
        {
            _fireNotificationSender = fireNotificationSender;
            _firebaseConfiguration = firebaseConfiguration;
        }

        //todo authorize
        [HttpPost]
        public async Task<ActionResult> Send([FromBody] SendFirebaseDto request)
        {
            if (!Request.Headers.Any(x => x.Key == "secret" && x.Value == _firebaseConfiguration.ProxySecret))
                return Unauthorized();

            return Ok(await _fireNotificationSender.Send(request.Addresses[0], request.Title, request.Body, request.Url, request.Data, new Guid()));
        }

    }
}