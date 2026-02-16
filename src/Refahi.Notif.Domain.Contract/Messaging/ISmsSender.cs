using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Domain.Contract.Messaging
{
    //public delegate ISmsSender SmsServiceResolver(bool isResend, bool isAudio, SmsGateway? gateway);
    public interface ISmsSender
    {
        SmsGateway Gateway { get; }

        /// <summary>
        /// مقدار بولین تعیین می کند که درخواست کلا غیرقابل ارسال است
        /// </summary>
        /// <param name="phoneNumbers"></param>
        /// <param name="message"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        Task<(string, bool)> SendAsync(string[] phoneNumbers, string message, string? sender);

        Task<string> VerifyAsync(VerifySmsTemplate template, string phoneNumber, string code1, string? code2 = null,
            string? code3 = null, bool needTag = true);
        Task<string> VerifyAudioAsync(VerifySmsTemplate template, string phoneNumber, string code1, string? code2 = null, string? code3 = null);
    }


}
