using Refahi.Notif.Messages.NotifCenter;
using MassTransit;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.EndPoint.Razor.Pages
{
    public class SendMessageModel : PageModel
    {
        private readonly IBus _bus;

        public SendMessageModel(IBus bus)
        {
            _bus = bus;
        }

        public async Task OnPostAsync()
        {
            return;
            //try
            //{
            await _bus.Publish(new SendVerifySms
            {
                Code = Request.Form["code"],
                ExpireTime = DateTime.Now.AddMinutes(3),
                PhoneNumber = Request.Form["phonenumber"],
                Template = (VerifySmsTemplate)int.Parse(Request.Form["template"])
            });
            //}
            //catch (Exception ex)
            //{
            //    ViewData["Error"] = ex.Message;
            //}
        }
        public async Task OnPostMessageAsync()
        {
            return;
            //try
            //{
            SmsGateway? Gateway = null;
            if (int.TryParse(Request.Form["smsgateway"], out var gateWayNumber))
            {
                Gateway = (SmsGateway)gateWayNumber;
            }
            await _bus.Publish(new SendMessage
            {
                Id = Guid.NewGuid(),
                Sms = new SendSmsRequest
                {
                    Gateway = Gateway,
                    Body = Request.Form["body"],
                    PhoneNumbers = new[] { Request.Form["phonenumber"].ToString() }
                }

            });
            //}
            //catch (Exception ex)
            //{
            //    ViewData["Error"] = ex.Message;
            //}
        }
    }
}
