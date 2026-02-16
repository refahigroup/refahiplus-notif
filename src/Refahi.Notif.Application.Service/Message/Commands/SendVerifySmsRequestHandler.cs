using MediatR;
using Refahi.Notif.Application.Contract.Dtos.Message.Commands;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Infrastructure.Messaging.Sms;

namespace Refahi.Notif.Application.Service.Message.Commands
{
    public class SendVerifySmsRequestHandler : IRequestHandler<SendVerifySmsRequest, string>
    {
        private readonly ISmsSenderFactory _smsSenderFactory;
        private readonly SmsTemplate _smsConfiguration;
        public SendVerifySmsRequestHandler(SmsTemplate smsConfiguration, ISmsSenderFactory smsSenderFactory)
        {
            _smsConfiguration = smsConfiguration;
            _smsSenderFactory = smsSenderFactory;
        }
        public async Task<string> Handle(SendVerifySmsRequest request, CancellationToken cancellationToken)
        {
            var smsSender = _smsSenderFactory.GetServiceForVerify(request.IsAudio, request.IsResend);



            if (!request.IsAudio)
            {
                if (!request.IsResend)
                    return (await smsSender.SendAsync(new[] { request.PhoneNumber },
                        _smsConfiguration.GetVerifyMessage(request.Template, request.Code,
                            request.ServiceUrl ?? "refahiplus.com", request.NeedTag ?? true),
                        null)).Item1;

                return await smsSender.VerifyAsync(request.Template, request.PhoneNumber, request.Code, request.ServiceUrl, null, request.NeedTag ?? true);
            }
            return await smsSender.VerifyAudioAsync(request.Template, request.PhoneNumber, request.Code);
        }
    }
}
