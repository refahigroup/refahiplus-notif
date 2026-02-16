using AutoMapper;
using MediatR;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Contract.Dtos.VerifySms.Commands
{
    [AutoMap(typeof(VerifySmsSent))]
    public class AddVerifyMessageRequest : VerifySmsSent, IRequest
    {

    }
}
