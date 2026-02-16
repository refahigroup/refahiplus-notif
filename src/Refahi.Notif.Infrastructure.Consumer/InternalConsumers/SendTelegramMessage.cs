using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Infrastructure.Consumer.InternalConsumers
{
    public class SendTelegramMessageConsumer :
        IConsumer<SendTelegramMessage>
    {
        readonly ILogger<SendTelegramMessageConsumer> _logger;
        readonly IMediator _mediator;
        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IFileService _fileService;
        readonly ITelegramSender _telegramSender;

        public SendTelegramMessageConsumer(ILogger<SendTelegramMessageConsumer> logger, IMediator mediator, IMapper mapper, IUnitOfWork unitOfWork, ITelegramSender telegramSender, IFileService fileService)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _telegramSender = telegramSender;
            _fileService = fileService;
        }

        public async Task Consume(ConsumeContext<SendTelegramMessage> context)
        {
            try
            {
                var domain = await _unitOfWork.MessageRepository.GetAsync(context.Message.MessageId);
                if (domain == null || domain.Telegram == null || domain.Telegram?.RetryCount >= 3)
                    return;

                var Telegram = domain.Telegram;
                var validatorOk = true;
                string sendResult = "";

                var done = false;
                try
                {
                    if (!string.IsNullOrEmpty(domain.ValidatorUrl))
                        validatorOk = await CheckTelegramValidator(domain.ValidatorUrl);
                    if (validatorOk && domain.Telegram?.ValidatorUrl != null && domain.Telegram?.ValidatorUrl.Length > 0)
                    {
                        foreach (var url in domain.Telegram.ValidatorUrl)
                        {
                            validatorOk = await CheckTelegramValidator(url);
                            if (!validatorOk)
                                break;
                        }
                    }

                    if (validatorOk)
                    {
                        Stream? fileData = null;
                        if (!string.IsNullOrEmpty(Telegram.FileId))
                        {
                            fileData = await _fileService.Download(Telegram.FileId);
                        }

                        sendResult = await _telegramSender.SendAsync(Telegram.ChatId, Telegram.Body, Telegram.FileName,
                            fileData);
                        done = true;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error In Set Telegram Status : {ex.Message}");
                    throw;
                }
                finally
                {
                    try
                    {
                        if (!validatorOk)
                            domain.TelegramValidatorDeny();
                        else if (done)
                            domain.TelegramSend(sendResult);
                        else
                            domain.TelegramRetry();
                        _unitOfWork.MessageRepository.Update(domain);
                        await _unitOfWork.SaveAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error In Consume SendTelegramMessageConsumer: {Message},Error:{Error}", context.Message.Serilize(), ex.GetFullError());
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error In Consume SendTelegramMessage: {Message},Error:{Error}", context.Message.Serilize(), ex.Message);

                throw;
            }
        }
        private static async Task<bool> CheckTelegramValidator(string url)
        {
            if (string.IsNullOrEmpty(url))
                return true;
            using var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.GetAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }



    }
}
