using MediatR;
using Refahi.Notif.Application.Contract.Dtos.Message.Commands;
using Refahi.Notif.Domain.Contract.Repositories;

namespace Refahi.Notif.Application.Service.Message.Commands
{
    public class SmsDeliveryChangeRequestHandler : IRequestHandler<SmsDeliveryChangeRequest>
    {
        private readonly IUnitOfWork _unitOfWork;


        public SmsDeliveryChangeRequestHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(SmsDeliveryChangeRequest request, CancellationToken cancellationToken)
        {
            if (await CheckMessage(request) || await CheckVerifyMessage(request))
                await _unitOfWork.SaveAsync();
        }
        private async Task<bool> CheckMessage(SmsDeliveryChangeRequest request)
        {
            var message = await _unitOfWork.MessageRepository.GetBySmsIdInProviderAsync(request.IdInProvider);
            if (message == null)
                return false;

            if (request.IsDelivered)
                message.SmsDelivered();
            else
                message.SmsUnDelivered();

            _unitOfWork.MessageRepository.Update(message);
            return true;
        }

        private async Task<bool> CheckVerifyMessage(SmsDeliveryChangeRequest request)
        {
            var verifyMessage = await _unitOfWork.VerifyMessageRepository.GetByIdInProviderAsync(request.IdInProvider);
            if (verifyMessage == null)
                return false;

            if (request.IsDelivered)
                verifyMessage.Delivered();
            else
                verifyMessage.UnDelivered();

            _unitOfWork.VerifyMessageRepository.Update(verifyMessage);
            return true;
        }
    }
}
