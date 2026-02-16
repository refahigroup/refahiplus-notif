using Refahi.Notif.Messages.NotifCenter;
using Hangfire;
using MassTransit;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Domain.Contract.Repositories;

namespace Refahi.Notif.Application.Service.Message.Common
{
    public interface IMessageService
    {
        Task AddJobAndSetToDomain(Domain.Core.Aggregates.MessageAgg.Message domain);
        Task AddJobAndSetToDomainSeparate(Domain.Core.Aggregates.MessageAgg.Message domain);
    }
    public class MessageService : IMessageService
    {
        private readonly ILogger<MessageService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBus _bus;
        public MessageService(ILogger<MessageService> logger, IUnitOfWork unitOfWork, IBus bus)
        {
            _bus = bus;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task AddJobAndSetToDomain(Domain.Core.Aggregates.MessageAgg.Message domain)
        {
            try
            {

                if (domain.DueTime.HasValue)
                {
                    var jobId = AddJob(domain);
                    domain.Enqueued(jobId);
                    _unitOfWork.MessageRepository.Update(domain);
                    await _unitOfWork.SaveAsync();
                }
                else
                {
                    domain.Enqueued(null);
                    _unitOfWork.MessageRepository.Update(domain);
                    await _unitOfWork.SaveAsync();

                    await JobAction(
                     domain.Id,
                     domain.Sms != null,
                     domain.Email != null,
                     domain.Telegram != null,
                     domain.PushNotification != null,
                     domain.Notification != null);
                }


            }
            catch (Exception ex)
            {
                _unitOfWork.MessageRepository.Delete(domain);
                throw;

            }
            finally
            {
                await _unitOfWork.SaveAsync();
            }
        }
        private string AddJob(Domain.Core.Aggregates.MessageAgg.Message domain)
        {
            var delay = domain.DueTime.Value - DateTime.Now;

            return BackgroundJob.Schedule(() =>
                    JobAction(
                        domain.Id,
                        domain.Sms != null,
                        domain.Email != null,
                        domain.Telegram != null,
                        domain.PushNotification != null,
                        domain.Notification != null)
                , delay);
        }


        public async Task JobAction(Guid messageId, bool haveSms, bool haveEmail, bool haveTelegram, bool havePush, bool haveNotification)
        {
            if (haveSms)
                await SmsJobAction(messageId);
            if (haveEmail)
                await EmailJobAction(messageId);
            if (haveTelegram)
                await TelegramJobAction(messageId);
            if (havePush)
                await PushJobAction(messageId);
            if (haveNotification)
                await NotificationJobAction(messageId);
        }
        public async Task SmsJobAction(Guid messageId) => await _bus.Publish(new SendSmsMessage { MessageId = messageId });
        public async Task EmailJobAction(Guid messageId) => await _bus.Publish(new SendEmailMessage { MessageId = messageId });
        public async Task TelegramJobAction(Guid messageId) => await _bus.Publish(new SendTelegramMessage { MessageId = messageId });
        public async Task PushJobAction(Guid messageId) => await _bus.Publish(new SendPushMessage { MessageId = messageId });
        public async Task NotificationJobAction(Guid messageId) => await _bus.Publish(new SendNotificationMessage { MessageId = messageId });

        public async Task AddJobAndSetToDomainSeparate(Domain.Core.Aggregates.MessageAgg.Message domain)
        {
            try
            {
                string? jobId = null;
                if (domain.PushNotification is { DueTime: not null })
                    jobId = BackgroundJob.Schedule(() => PushJobAction(domain.Id),
                        domain.PushNotification.DueTime.Value - DateTime.Now);
                if (domain.Notification is { DueTime: not null })
                    jobId = BackgroundJob.Schedule(() => NotificationJobAction(domain.Id),
                        domain.Notification.DueTime.Value - DateTime.Now);
                if (domain.Sms is { DueTime: not null })
                    jobId = BackgroundJob.Schedule(() => SmsJobAction(domain.Id),
                        domain.Sms.DueTime.Value - DateTime.Now);
                if (domain.Email is { DueTime: not null })
                    jobId = BackgroundJob.Schedule(() => EmailJobAction(domain.Id),
                        domain.Email.DueTime.Value - DateTime.Now);
                if (domain.Telegram is { DueTime: not null })
                    jobId = BackgroundJob.Schedule(() => TelegramJobAction(domain.Id),
                        domain.Telegram.DueTime.Value - DateTime.Now);
                domain.Enqueued(jobId);
                _unitOfWork.MessageRepository.Update(domain);
                await _unitOfWork.SaveAsync();

                if (domain.PushNotification != null && !domain.PushNotification.DueTime.HasValue) await PushJobAction(domain.Id);
                if (domain.Sms != null && !domain.Sms.DueTime.HasValue) await SmsJobAction(domain.Id);
                if (domain.Notification != null && !domain.Notification.DueTime.HasValue) await NotificationJobAction(domain.Id);
                if (domain.Email != null && !domain.Email.DueTime.HasValue) await EmailJobAction(domain.Id);
                if (domain.Telegram != null && !domain.Telegram.DueTime.HasValue) await TelegramJobAction(domain.Id);
            }
            catch (Exception ex)
            {
                _unitOfWork.MessageRepository.Delete(domain);
                throw;

            }
            finally
            {
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
