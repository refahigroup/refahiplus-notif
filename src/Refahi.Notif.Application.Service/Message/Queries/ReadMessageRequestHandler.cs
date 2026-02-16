using MediatR;
using Refahi.Notif.Application.Contract.Dtos.Message.Queries;
using Refahi.Notif.Domain.Contract.Models;
using Refahi.Notif.Domain.Contract.Repositories;

namespace Refahi.Notif.Application.Service.Message.Queries
{
    public class ReadMessageRequestHandler : IRequestHandler<ReadMessageRequest, MessageModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReadMessageRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<MessageModel> Handle(ReadMessageRequest request, CancellationToken cancellationToken)
        {
            return _unitOfWork.MessageRepository.GetModelAsync(request.Id);
        }
    }
}
