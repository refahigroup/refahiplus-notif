using MediatR;
using Refahi.Notif.Application.Contract.Dtos.Message.Queries;
using Refahi.Notif.Domain.Contract.Models;
using Refahi.Notif.Domain.Contract.Repositories;

namespace Refahi.Notif.Application.Service.Message.Queries
{
    public class ReadMessageListHandler : IRequestHandler<ReadMessageListRequest, List<MessageModel>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReadMessageListHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<List<MessageModel>> Handle(ReadMessageListRequest request, CancellationToken cancellationToken)
        {
            return _unitOfWork.MessageRepository.WithPhoneNumber(request.PhoneNumber);
        }
    }
}
