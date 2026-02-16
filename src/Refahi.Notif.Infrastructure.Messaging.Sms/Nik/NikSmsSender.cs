using Microsoft.Extensions.Logging;
using NikSms.Library.NetCore.ViewModels;
using NikSms.Library.NetCore.WebApi;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Infrastructure.Messaging.Sms;
using Refahi.Notif.Messages.NotifCenter.Enums;
using System.Text.Json;

namespace Refahi.Notif.Infrastructure.Messaging.Sms.Nik
{
    public class NikSmsSender : ISmsSender
    {
        private readonly PublicApiV1 _nikService;
        private readonly NikSmsConfiguration _nikSmsConfig;
        private readonly SmsTemplate _smsConfig;
        private readonly ILogger<NikSmsSender> _logger;
        //todo delivery callback
        public NikSmsSender(NikSmsConfiguration nikSmsConfig, SmsTemplate smsComfig, ILogger<NikSmsSender> logger)
        {
            _nikSmsConfig = nikSmsConfig;
            _smsConfig = smsComfig;
            _nikService = new PublicApiV1(nikSmsConfig.UserName, nikSmsConfig.Password, "fa");
            _logger = logger;
        }

        public SmsGateway Gateway => SmsGateway.Niksms;

        public async Task<(string, bool)> SendAsync(string[] phoneNumbers, string message, string? sender)
        {
            var result = await _nikService.PtpSms(
                                    string.IsNullOrEmpty(sender) ? _nikSmsConfig.Sender : sender,
                                     phoneNumbers.ToList(),
                                     new List<string> { message });
            if (result.Status != LibOperationResultStatus.Success)
                throw result.Exception;

            //try
            //{

            _logger.LogInformation($"Done In Send By NikSms : {phoneNumbers.First()}, {result.Data}");

            var response = JsonSerializer.Deserialize<NikSmsSendResultModel>(result.Data);
            if (response.Status != 1)
                throw new Exception($"Error In Send By NikSms : Status is {response.Status}");

            return (response.Id, false);
            //}
            //catch(Exception ex)
            //{
            //    _logger.LogError(ex, $"Error In Send By NikSms : {phoneNumbers}");
            //    return (null,false);
            //}
        }

        public async Task<string> VerifyAsync(VerifySmsTemplate template, string phoneNumber, string code1,
            string? code2 = null, string? code3 = null, bool needTag = false)
        {
            var message = _smsConfig.GetVerifyMessage(template, code1, code2, needTag);
            return (await SendAsync(new string[] { phoneNumber }, message, null)).Item1;
        }

        public Task<string> VerifyAudioAsync(VerifySmsTemplate template, string phoneNumber, string code1, string? code2 = null, string? code3 = null)
        {
            throw new NotImplementedException();
        }
    }
    public class NikSmsSendResultModel
    {
        public string Id { get; set; }
        public int Status { get; set; }
    }
}
