using MediatR;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Application.Contract.Dtos.User.Commands;
using Refahi.Notif.Domain.Contract.Repositories;

namespace Refahi.Notif.Application.Service.User.Commands
{
    public class SetUserPhoneNumberRequestHandler : IRequestHandler<SetUserPhoneNumberRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SetUserPhoneNumberRequestHandler> _logger;
        public SetUserPhoneNumberRequestHandler(IUnitOfWork unitOfWork, ILogger<SetUserPhoneNumberRequestHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task Handle(SetUserPhoneNumberRequest request, CancellationToken cancellationToken)
        {
            try
            {
                //identity allow duplicate phone number
                //var isExist = await _unitOfWork.UserRepository.IsExistByPhoneNumberNotUserIdAsync(request.PhoneNumber, request.UserId);
                //if (isExist)
                //{
                //    _logger.LogError($"Can't Handle Request : {request.Serilize()} ; Error :{Errors.PhoneNumberExist}");
                //    return Unit.Value;
                //}


                var user = await _unitOfWork.UserRepository.GetAsync(request.UserId);
                bool userNotExist = user == null;


                if (userNotExist)
                    user = new Domain.Core.Aggregates.UserAgg.User(request.UserId);

                user.SetPhoneNumber(request.PhoneNumber);

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
