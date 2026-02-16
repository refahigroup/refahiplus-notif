using MediatR;
using Refahi.Notif.Application.Contract.Dtos.Message.Queries;
using Refahi.Notif.Domain.Contract.Models;
using Refahi.Notif.Domain.Contract.Repositories;

namespace Refahi.Notif.Application.Service.Message.Queries
{
    public class ReadMessageByUserIdRequestHandler : IRequestHandler<ReadMessageByUserIdRequest, List<MessageModel>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public ReadMessageByUserIdRequestHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<MessageModel>> Handle(ReadMessageByUserIdRequest request, CancellationToken cancellationToken)
        {
            var list = await _unitOfWork.MessageRepository.GetByUserIdSended(request.UserId);
            return list;
        }
    }
}
