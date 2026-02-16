using MediatR;
using Refahi.Notif.Domain.Contract.Models;

namespace Refahi.Notif.Application.Contract.Dtos.Message.Queries
{
    public class ReadMessageByUserIdRequest : IRequest<List<MessageModel>>
    {
        public long UserId { get; set; }
    }


}
