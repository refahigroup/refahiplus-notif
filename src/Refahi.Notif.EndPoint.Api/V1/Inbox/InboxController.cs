using Refahi.Notif.Application.Contract.Dtos.Inbox.Commands;
using Refahi.Notif.Application.Contract.Dtos.Inbox.Queries;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Refahi.Notif.Application.Contract.Services;
using Refahi.Notif.Domain.Core.Aggregates.UserAgg.Entities;
using Refahi.Notif.EndPoint.Api.V1;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.EndPoint.Api.V1.Inbox
{
    public class InboxController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        public InboxController(IMediator mediator, IIdentityService identityService)
        {
            _mediator = mediator;
            _identityService = identityService;
        }
        [HttpGet("messages")]
        public async Task<ActionResult> GetUserInboxMessages()
        {
            var result = await _mediator.Send(new GetUserInboxMessagesQuery() { UserId = _identityService.UserId });
            return Ok(result);
        }
        [HttpGet("messages/phr")]
        public async Task<ActionResult> GetUserPhrInboxMessages(int? pageNumber, int? pageSize)
        {
            var result = await _mediator.Send(new GetUserInboxMessagesQuery()
            {
                UserId = _identityService.UserId,
                App = AppName.PHR,
                PageNumber = pageNumber ?? 1,
                PageSize = pageSize ?? 20
            });
            return Ok(result);
        }
        [HttpGet("messages/phr/reminder")]
        public async Task<ActionResult> GetUserPhrReminderInboxMessages(int? pageNumber, int? pageSize)
        {
            var result = await _mediator.Send(new GetUserInboxMessagesQuery()
            {
                UserId = _identityService.UserId,
                App = AppName.PHRReminder,
                PageNumber = pageNumber ?? 1,
                PageSize = pageSize ?? 20
            });
            return Ok(result);
        }
        [HttpGet("messages/emr")]
        public async Task<ActionResult> GetUserEmrInboxMessages(int? pageNumber, int? pageSize)
        {
            var result = await _mediator.Send(new GetUserInboxMessagesQuery()
            {
                UserId = _identityService.UserId,
                App = AppName.EMR,
                PageNumber = pageNumber ?? 1,
                PageSize = pageSize ?? 20
            });
            return Ok(result);
        }
        [HttpGet("messages/phr/list")]
        public async Task<ActionResult> GetUserPhrInboxMessagesList(int? pageNumber, int? pageSize)
        {
            var result = await _mediator.Send(new GetUserInboxMessagesQuery()
            {
                UserId = _identityService.UserId,
                App = AppName.PHR,
                PageNumber = pageNumber ?? 1,
                PageSize = pageSize ?? 20
            });
            return Ok(result);
        }
        [HttpGet("messages/phr/reminder/list")]
        public async Task<ActionResult> GetUserPhrReminderInboxMessagesList(int? pageNumber, int? pageSize)
        {
            var result = await _mediator.Send(new GetUserInboxMessagesQuery()
            {
                UserId = _identityService.UserId,
                App = AppName.PHRReminder,
                PageNumber = pageNumber ?? 1,
                PageSize = pageSize ?? 20
            });
            return Ok(result);
        }
        [HttpGet("messages/emr/list")]
        public async Task<ActionResult> GetUserEmrInboxMessagesList(int? pageNumber, int? pageSize)
        {
            var result = await _mediator.Send(new GetUserInboxMessagesQuery()
            {
                UserId = _identityService.UserId,
                App = AppName.EMR,
                PageNumber = pageNumber ?? 1,
                PageSize = pageSize ?? 20
            });
            return Ok(result);
        }
        [HttpGet("messages/phr/Count")]
        public async Task<ActionResult> GetUserPhrInboxMessagesCount()
        {
            var result = await _mediator.Send(new GetUserInboxMessagesCountQuery() { UserId = _identityService.UserId, App = AppName.PHR });
            return Ok(result);
        }
        [HttpGet("messages/phr/reminder/Count")]
        public async Task<ActionResult> GetUserPhrReminderInboxMessagesCount()
        {
            var result = await _mediator.Send(new GetUserInboxMessagesCountQuery() { UserId = _identityService.UserId, App = AppName.PHRReminder });
            return Ok(result);
        }
        [HttpGet("messages/emr/Count")]
        public async Task<ActionResult> GetUserEmrInboxMessagesCount()
        {
            var result = await _mediator.Send(new GetUserInboxMessagesCountQuery() { UserId = _identityService.UserId, App = AppName.EMR });
            return Ok(result);
        }
        [HttpPut("messages/phr/read")]
        public async Task<ActionResult> ReadUserPhrInbox()
        {
            await _mediator.Send(new SetUserInboxAsReadCommand(_identityService.UserId, AppName.PHR));
            return Ok();
        }
        [HttpPut("messages/phr/reminder/read")]
        public async Task<ActionResult> ReadUserPhrReminderInbox()
        {
            await _mediator.Send(new SetUserInboxAsReadCommand(_identityService.UserId, AppName.PHRReminder));
            return Ok();
        }
        [HttpPut("messages/emr/read")]
        public async Task<ActionResult> ReadUserEmrInbox()
        {
            await _mediator.Send(new SetUserInboxAsReadCommand(_identityService.UserId, AppName.EMR));
            return Ok();
        }
        [HttpPut("messages/emr/{messageId}/read")]
        public async Task<ActionResult> ReadUserEmrInboxMessage([FromRoute] Guid messageId)
        {
            await _mediator.Send(new SetUserInboxMessageAsReadCommand(messageId, _identityService.UserId, AppName.EMR));
            return Ok();
        }
        [HttpPut("messages/phr/{messageId}/read")]
        public async Task<ActionResult> ReadUserPhrInboxMessage([FromRoute] Guid messageId)
        {
            await _mediator.Send(new SetUserInboxMessageAsReadCommand(messageId, _identityService.UserId, AppName.PHR));
            return Ok();
        }
        [HttpPut("messages/phr/reminder/{messageId}/hide")]
        public async Task<ActionResult> HideUserPhrReminderInboxMessage([FromRoute] Guid messageId)
        {
            await _mediator.Send(new SetUserInboxMessageNewStatusCommand(messageId, _identityService.UserId, AppName.PHRReminder, InboxMessageStatus.Hide));
            return Ok();
        }
        [HttpPut("messages/phr/reminder/{messageId}/done")]
        public async Task<ActionResult> DoneUserPhrReminderInboxMessage([FromRoute] Guid messageId)
        {
            await _mediator.Send(new SetUserInboxMessageNewStatusCommand(messageId, _identityService.UserId, AppName.PHRReminder, InboxMessageStatus.Done));
            return Ok();
        }
    }
}
