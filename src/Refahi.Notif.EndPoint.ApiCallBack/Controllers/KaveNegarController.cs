using Refahi.Notif.Messages.NotifCenter;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Refahi.Notif.Infrastructure.Messaging.Sms.KaveNegar;

namespace Refahi.Notif.EndPoint.ApiCallBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KaveNegarController : ControllerBase
    {

        private readonly ILogger<KaveNegarController> _logger;
        private readonly IBus _bus;
        public KaveNegarController(ILogger<KaveNegarController> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }


        [HttpGet("KaveSmsDeliveryCallback")]
        public async Task<ActionResult> KaveSmsDeliveryCallback(string messageid, KaveSmsStatus status)
        {
            await _bus.Publish(new SmsDeliveryChange
            {
                IdInProvider = messageid,
                IsDelivered = status.IsDelivered()
            });
            return Ok();
        }

    }
}