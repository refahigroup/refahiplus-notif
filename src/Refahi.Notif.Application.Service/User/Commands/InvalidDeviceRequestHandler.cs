using MediatR;
using Refahi.Notif.Application.Contract.Dtos.User.Commands;
using Refahi.Notif.Domain.Contract.Repositories;

namespace Refahi.Notif.Application.Service.User.Commands
{
    public class InvalidDeviceRequestHandler : IRequestHandler<InvalidDeviceRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvalidDeviceRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(InvalidDeviceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.UserRepository.DeleteDeviceByToken(request.NotificationToken);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
