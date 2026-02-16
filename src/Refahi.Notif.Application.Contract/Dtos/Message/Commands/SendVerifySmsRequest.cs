using AutoMapper;
using MediatR;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Contract.Dtos.Message.Commands
{
    [AutoMap(typeof(SendVerifySms), ReverseMap = true)]
    public class SendVerifySmsRequest : SendVerifySms, IRequest<string>
    {

    }
}
