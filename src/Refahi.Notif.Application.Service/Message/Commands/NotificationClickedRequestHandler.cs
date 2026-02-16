using Refahi.Notif.Domain.Core.Exceptions;
using MediatR;
using Refahi.Notif.Application.Contract.Dtos.Message.Commands;
using Refahi.Notif.Domain.Contract.Repositories;

namespace Refahi.Notif.Application.Service.Message.Commands
{
    public class NotificationClickedRequestHandler : IRequestHandler<NotificationClickedRequest>
    {
        private readonly IUnitOfWork _unitOfWork;


        public NotificationClickedRequestHandler(
            IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(NotificationClickedRequest request, CancellationToken cancellationToken)
        {
            var message = await _unitOfWork.MessageRepository.GetAsync(request.Id);
            if (message == null)
                throw new BussinessException(Errors.MessageNotFound);

            message.PushNotificationClicked(request.FCMMessageId);
            _unitOfWork.MessageRepository.Update(message);
            await _unitOfWork.SaveAsync();
        }

    }
}
