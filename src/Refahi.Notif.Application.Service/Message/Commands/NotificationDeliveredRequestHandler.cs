using MediatR;
using Refahi.Notif.Application.Contract.Dtos.Message.Commands;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Domain.Core.Exceptions;

namespace Refahi.Notif.Application.Service.Message.Commands
{
    public class NotificationDeliveredRequestHandler : IRequestHandler<NotificationDeliveredRequest>
    {
        private readonly IUnitOfWork _unitOfWork;


        public NotificationDeliveredRequestHandler(
            IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(NotificationDeliveredRequest request, CancellationToken cancellationToken)
        {
            var message = await _unitOfWork.MessageRepository.GetAsync(request.Id);
            if (message == null)
                throw new BussinessException(Errors.MessageNotFound);

            message.PushNotificationDelivered(request.FCMMessageId);
            _unitOfWork.MessageRepository.Update(message);
            await _unitOfWork.SaveAsync();
        }

    }
}
