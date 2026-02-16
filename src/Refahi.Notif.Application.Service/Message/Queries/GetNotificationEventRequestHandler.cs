using MediatR;
using Refahi.Notif.Application.Contract.Dtos.Message.Queries;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Domain.Core.Aggregates.MessageAgg.Entities;

namespace Refahi.Notif.Application.Service.Message.Queries
{
    public class GetNotificationEventRequestHandler : IRequestHandler<GetNotificationEventRequest, List<NotificationEvent>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetNotificationEventRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<List<NotificationEvent>> Handle(GetNotificationEventRequest request, CancellationToken cancellationToken)
        {
            return _unitOfWork.NotificationEventRepository.GetListByNotificationIdAsync(request.Id);
        }
    }
}
