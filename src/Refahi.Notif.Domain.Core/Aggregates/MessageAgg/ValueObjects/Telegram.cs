
using Refahi.Notif.Domain.Core.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Refahi.Notif.Domain.Core.Aggregates.MessageAgg.ValueObjects
{

    public class Telegram
    {
        public string ChatId { get; private init; }
        public string? Body { get; private init; }
        public string? FileName { get; private init; }
        public string? FileId { get; private init; }
        public string? SendResult { get; private init; }
        public DateTime? DueTime { get; private init; }
        public string[]? ValidatorUrl { get; private init; }
        public TelegramStatus Status { get; private init; }
        public int RetryCount { get; private init; }

        public Telegram(string chatId, string? body, string? fileName, string? fileId)
        {
            ChatId = chatId;
            Body = body;
            FileName = fileName;
            FileId = fileId;
            Status = TelegramStatus.Created;
            RetryCount = 0;
        }
        public Telegram(TelegramStatus status, int retryCount, string chatId, string? body, string? fileName,
            string? fileId, DateTime? dueTime, string[]? validatorUrl, string? sendResult)
        {
            ChatId = chatId;
            Body = body;
            FileName = fileName;
            FileId = fileId;
            Status = status;
            DueTime = dueTime;
            ValidatorUrl = validatorUrl;
            RetryCount = retryCount;
            SendResult = sendResult;
        }
        public Telegram(TelegramStatus status, string chatId, string? body, string? fileName, string? fileId,
            DateTime? dueTime, string[]? validatorUrl, string? sendResult)
        {
            ChatId = chatId;
            Body = body;
            FileName = fileName;
            FileId = fileId;
            Status = status;
            DueTime = dueTime;
            ValidatorUrl = validatorUrl;
            RetryCount = 0;
            SendResult = sendResult;
        }

        public Telegram(string chatId, string? body, string? fileName, string? fileId, DateTime? dueTime, string[]? validatorUrl)
        {
            ChatId = chatId;
            Body = body;
            FileName = fileName;
            FileId = fileId;
            DueTime = dueTime;
            ValidatorUrl = validatorUrl;
            Status = TelegramStatus.Created;
            RetryCount = 0;
        }

        internal Telegram Send(string result)
        {
            return new Telegram(TelegramStatus.Sended, RetryCount, ChatId, Body, FileName, FileId, DueTime, ValidatorUrl, result);
        }
        internal Telegram Retry()
        {
            if (Status != TelegramStatus.Pending)
                throw new BussinessException(Errors.TelegramStatusNotCorrect(Status));

            return new Telegram(Status, RetryCount + 1, ChatId, Body, FileName, FileId, DueTime, ValidatorUrl, SendResult);
        }

        internal Telegram Enqueued()
        {
            if (Status != TelegramStatus.Created)
                throw new BussinessException(Errors.TelegramStatusNotCorrect(Status));

            return new Telegram(TelegramStatus.Pending, ChatId, Body, FileName, FileId, DueTime, ValidatorUrl, SendResult);
        }
        public Telegram ValidatorDeny()
        {
            return new Telegram(TelegramStatus.InvalidDeny, ChatId, Body, FileName, FileId, DueTime, ValidatorUrl, SendResult);
        }



    }
    public enum TelegramStatus
    {
        [Display(Name = "در حال پردازش")]
        Created = 0,
        [Display(Name = "در انتظار ارسال")]
        Pending = 1,

        [Display(Name = "ارسال شده")]
        Sended = 2,

        [Display(Name = "تحویل شده")]
        Delivered = 3,

        [Display(Name = "کلیک شده")]
        Clicked = 4,

        [Display(Name = "لغو شده")]
        InvalidDeny = 6,
    }
}
