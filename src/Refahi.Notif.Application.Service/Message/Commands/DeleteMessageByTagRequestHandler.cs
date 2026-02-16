using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Application.Contract.Dtos.Message.Commands;
using Refahi.Notif.Domain.Contract.Repositories;

namespace Refahi.Notif.Application.Service.Message.Commands
{
    public class DeleteMessageByTagRequestHandler : IRequestHandler<DeleteMessageByTagRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteMessageByTagRequestHandler> _logger;


        public DeleteMessageByTagRequestHandler(
            IUnitOfWork unitOfWork,
            ILogger<DeleteMessageByTagRequestHandler> logger
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task Handle(DeleteMessageByTagRequest request, CancellationToken cancellationToken)
        {
            if (request.DeleteDirect == true)
            {
                await _unitOfWork.MessageRepository.DeleteByTag(request.Tag);
                return;
            }
            var messages = await _unitOfWork.MessageRepository.GetByTag(request.Tag);
            var jobIds = new HashSet<string>();
            foreach (var message in messages)
            {
                if (message == null || !message.DueTime.HasValue || message.DueTime < DateTime.Now)
                    continue;

                _unitOfWork.MessageRepository.Delete(message);

                try
                {
                    BackgroundJob.Delete(message.JobId);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error In Delete BackgroundJob : {ex.Message}");
                    //dont throw because backgroundjob without domain not work
                }

            }
            await _unitOfWork.SaveAsync();
        }
    }
}
