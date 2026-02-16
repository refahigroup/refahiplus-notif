using MediatR;
using Refahi.Notif.Application.Contract.Dtos.User.Commands;
using Refahi.Notif.Domain.Contract.Repositories;

namespace Refahi.Notif.Application.Service.User.Commands
{
    public class UpsertDeviceRequestHandler : IRequestHandler<UpsertDeviceRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertDeviceRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(UpsertDeviceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var userExistDevice = await _unitOfWork.UserRepository.GetByDeviceIdOrFirebaseTokenNotUserIdAsync(request.DeviceId, request.NotificationToken, request.UserId);
                if (userExistDevice != null)
                {
                    foreach (var device in userExistDevice.Devices.Where(x => x.Id == request.DeviceId || x.NotificationToken == request.NotificationToken).ToList())
                        userExistDevice.RemoveDevice(device.Id);

                    await _unitOfWork.UserRepository.UpdateWithRelatedEntities(userExistDevice);
                    await _unitOfWork.SaveAsync();
                }


                var user = await _unitOfWork.UserRepository.GetAsync(request.UserId);
                bool userNotExist = user == null;


                if (userNotExist)
                    user = new Domain.Core.Aggregates.UserAgg.User(request.UserId);

                user.UpsertDevice(request.DeviceId, request.NotificationToken, request.Type, request.App);

                if (userNotExist)
                {
                    await _unitOfWork.UserRepository.AddAsync(user);
                }
                else
                {
                    await _unitOfWork.UserRepository.UpdateWithRelatedEntities(user);
                }
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
