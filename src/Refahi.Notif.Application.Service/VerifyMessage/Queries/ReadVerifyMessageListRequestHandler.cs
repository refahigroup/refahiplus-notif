using MediatR;
using Refahi.Notif.Application.Contract.Dtos.VerifySms.Queries;
using Refahi.Notif.Domain.Contract.Models;
using Refahi.Notif.Domain.Contract.Repositories;

namespace Refahi.Notif.Application.Service.VerifyMessage.Queries
{
    public class ReadVerifyMessageListRequestHandler : IRequestHandler<ReadVerifyMessageListRequest, List<VerifyMessageModel>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public ReadVerifyMessageListRequestHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<List<VerifyMessageModel>> Handle(ReadVerifyMessageListRequest request, CancellationToken cancellationToken)
        {
            return _unitOfWork.VerifyMessageRepository.GetModelsByPhoneNumberAsync(request.PhoneNumber);
        }
    }
}
