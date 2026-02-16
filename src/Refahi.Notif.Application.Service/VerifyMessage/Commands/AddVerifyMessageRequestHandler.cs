using MediatR;
using Refahi.Notif.Application.Contract.Dtos.VerifySms.Commands;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Application.Service.VerifyMessage.Commands
{
    public class AddVerifyMessageRequestHandler : IRequestHandler<AddVerifyMessageRequest>
    {
        private readonly IUnitOfWork _unitOfWork;


        public AddVerifyMessageRequestHandler(
            IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(AddVerifyMessageRequest request, CancellationToken cancellationToken)
        {
            var verifyMessage = new Domain.Core.Aggregates.VerifyMessageAgg.VerifyMessage(request.Id, request.PhoneNumber, request.Template, request.SendTime, request.IdInProvider, SmsStatus.Sended, request.IsAudio);


            await _unitOfWork.VerifyMessageRepository.AddAsync(verifyMessage);
            await _unitOfWork.SaveAsync();
        }

    }
}
