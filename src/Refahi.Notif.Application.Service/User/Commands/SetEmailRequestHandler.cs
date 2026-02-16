using MediatR;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Application.Contract.Dtos.User.Commands;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Domain.Core.Exceptions;
using Refahi.Notif.Domain.Core.Utility;

namespace Refahi.Notif.Application.Service.User.Commands
{
    public class SetUserEmailRequestHandler : IRequestHandler<SetUserEmailRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SetUserEmailRequestHandler> _logger;
        public SetUserEmailRequestHandler(IUnitOfWork unitOfWork, ILogger<SetUserEmailRequestHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task Handle(SetUserEmailRequest request, CancellationToken cancellationToken)
        {
            try

            {
                var isExist = await _unitOfWork.UserRepository.IsExistByEmailNotUserIdAsync(request.Email, request.UserId);
                if (isExist)
                {
                    _logger.LogError($"Can't Handle Request : {request.Serilize()} ; Error :{Errors.EmailExist}");
                    return;
                }


                var user = await _unitOfWork.UserRepository.GetAsync(request.UserId);
                bool userNotExist = user == null;


                if (userNotExist)
                    user = new Domain.Core.Aggregates.UserAgg.User(request.UserId);

                user.SetEmail(request.Email);

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
