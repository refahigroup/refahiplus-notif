using AutoMapper;
using Refahi.Notif.Application.Contract.Dtos.Inbox.Queries;
using Refahi.Notif.Application.Contract.Dtos.Message.Queries;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Refahi.Notif.Application.Contract.Services;
using Refahi.Notif.Domain.Core.Aggregates.MessageAgg.ValueObjects;
using Refahi.Notif.Domain.Core.Aggregates.UserAgg.Entities;
using Refahi.Notif.EndPoint.Api.V1;

namespace Refahi.Notif.EndPoint.Api.V1.Message
{
    public class MessageValidationController : BaseController
    {
        private readonly IBus _bus;
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        readonly ILogger<MessageValidationController> _logger;
        private readonly IMapper _mapper;
        public MessageValidationController(IBus bus, IMediator mediator, IIdentityService identityService, IMapper mapper, ILogger<MessageValidationController> logger)
        {
            _bus = bus;
            _mediator = mediator;
            _identityService = identityService;
            _mapper = mapper;
            _logger = logger;
        }
        [AllowAnonymous]
        [HttpGet("Check/{id}/Sms")]
        public async Task<ActionResult> CheckSms(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new ReadMessageRequest { Id = id });
                if (result.Sms == null)
                    return NotFound();
                if (result.Sms.SendTime != null)
                    return BadRequest($"Message Sent At {result.Sms.SendTime}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Check/{id}/Sms" + "\n" + ex.Message);
            }

            return Ok();
        }
        [AllowAnonymous]
        [HttpGet("Check/{id}/Email")]
        public async Task<ActionResult> CheckEmail(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new ReadMessageRequest { Id = id });
                if (result.Email == null)
                    return NotFound();
                if (result.Email.SendTime != null)
                    return BadRequest($"Email Sent At {result.Email.SendTime}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Check/{id}/Email" + "\n" + ex.Message);
            }
            return Ok();
        }
        [AllowAnonymous]
        [HttpGet("Check/{id}/PushNotification")]
        public async Task<ActionResult> CheckPushNotification(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetNotificationEventRequest { Id = id });
                if (result != null && result.Any(p => p.EventName == "opened" || p.EventName == "closed"))
                    return BadRequest($"PushNotification Received");
                var result2 = await _mediator.Send(new ReadMessageRequest { Id = id });
                if (result2.PushNotification == null)
                    return NotFound();
                if (result2.PushNotification.Status == PushNotificationStatus.InvalidDeny)
                    return BadRequest($"Notification Sent Deny");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Check/{id}/PushNotification" + "\n" + ex.Message);
            }
            return Ok();
        }
        [AllowAnonymous]
        [HttpGet("Check/{id}/Notification")]
        public async Task<ActionResult> CheckNotification(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetInboxMessageQuery { Id = id });
                if (result == null)
                    return NotFound();
                if (result.ReadTime != null)
                    return BadRequest($"Notification Sent At {result.ReadTime}");
                if (result.Status == InboxMessageStatus.Hide)
                    return BadRequest($"Notification Set Hide {result.ReadTime}");
                if (result.Status == InboxMessageStatus.Done)
                    return BadRequest($"Notification Set Hide {result.ReadTime}");
                var result2 = await _mediator.Send(new ReadMessageRequest { Id = id });
                if (result2.Notification == null)
                    return NotFound();
                if (result2.Notification.Status == NotificationStatus.InvalidDeny)
                    return BadRequest($"Notification Sent Deny");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Check/{id}/Notification" + "\n" + ex.Message);
            }
            return Ok();
        }

    }
}