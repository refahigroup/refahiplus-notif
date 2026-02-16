using AutoMapper;
using MediatR;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Contract.Dtos.Message.Commands
{
    [AutoMap(typeof(SmsDeliveryChange), ReverseMap = true)]
    public class SmsDeliveryChangeRequest : SmsDeliveryChange, IRequest
    {

    }
}
