using MediatR;
using Refahi.Notif.Domain.Contract.Models;

namespace Refahi.Notif.Application.Contract.Dtos.VerifySms.Queries
{
    public class ReadVerifyMessageListRequest : IRequest<List<VerifyMessageModel>>
    {
        public string PhoneNumber { get; set; }
    }


}
