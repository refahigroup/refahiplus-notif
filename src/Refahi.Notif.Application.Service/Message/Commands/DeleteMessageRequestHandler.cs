using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Application.Contract.Dtos.Message.Commands;
using Refahi.Notif.Domain.Contract.Repositories;

namespace Refahi.Notif.Application.Service.Message.Commands
{
    public class DeleteMessageRequestHandler : IRequestHandler<DeleteMessageRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteMessageRequestHandler> _logger;


        public DeleteMessageRequestHandler(
            IUnitOfWork unitOfWork,
            ILogger<DeleteMessageRequestHandler> logger
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task Handle(DeleteMessageRequest request, CancellationToken cancellationToken)
        {
            var message = await _unitOfWork.MessageRepository.GetAsync(request.Id);
            if (message == null || !message.DueTime.HasValue || message.DueTime < DateTime.Now)
                return;


            _unitOfWork.MessageRepository.Delete(message);
            await _unitOfWork.SaveAsync();

            try
            {

                BackgroundJob.Delete(message.JobId);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error In Delete BackgroundJob : {ex.Message}");
                //dont throw because backgroundjob without domain not work
            }

            return;
        }
    }
}
