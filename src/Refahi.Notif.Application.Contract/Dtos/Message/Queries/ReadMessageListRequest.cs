using MediatR;
using Refahi.Notif.Domain.Contract.Models;

namespace Refahi.Notif.Application.Contract.Dtos.Message.Queries
{
    public class ReadMessageListRequest : IRequest<List<MessageModel>>
    {
        public string PhoneNumber { get; set; }
    }


}
