using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Refahi.Notif.Application.Contract.Dtos.Message.Commands;
using Refahi.Notif.Application.Contract.Dtos.Message.Queries;
using Refahi.Notif.Application.Contract.Services;
using Refahi.Notif.Domain.Contract.Models;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.EndPoint.Api.V1.Message
{
    public class MessageController : BaseController
    {
        private readonly IBus _bus;
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        public MessageController(IBus bus, IMediator mediator, IIdentityService identityService, IMapper mapper)
        {
            _bus = bus;
            _mediator = mediator;
            _identityService = identityService;
            _mapper = mapper;
        }

        //todo prevent external request
        [AllowAnonymous]
        [HttpPost("User")]
        public async Task<ActionResult> Send([FromBody] SendMessageToUserRequest request)
        {
            await _bus.Publish(_mapper.Map<SendMessageToUser>(request));
            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("Contact")]
        public async Task<ActionResult> SendContact([FromBody] SendMessageToContactRequest request)
        {
            await _bus.Publish(_mapper.Map<SendMessageToContact>(request));
            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("SendTelegram")]
        public async Task<ActionResult> SendTelegram(IFormFile file)
        {
            var request = new SendMessageToContactRequest();
            var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            request = new SendMessageToContactRequest()
            {
                Id = Guid.NewGuid(),
                TelegramMessage = new SendTelegramMessageToContactRequest()
                {
                    ChatId = "-1002226271189",
                    FileData = stream.ToArray(),
                    FileName = file.FileName
                }
            };
            await _bus.Publish(_mapper.Map<SendMessageToContact>(request));
            return Ok();
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Send([FromBody] SendMessageRequest request)
        {
            request = PopulateRequestForMonitoring(request);
            await _bus.Publish(_mapper.Map<SendMessage>(request));
            return Ok();
        }
        private SendMessageRequest PopulateRequestForMonitoring(SendMessageRequest request)
        {
            if (request.Id == Guid.Empty)
                request.Id = Guid.NewGuid();

            if (request.Sms != null)
                request.Sms.Body = request.Sms.Body?.Replace("{{time}}", DateTime.Now.ToString("HH:mm"));

            return request;
        }
        [AllowAnonymous]
        [HttpPost("User/RealTime")]
        public async Task<ActionResult> Send([FromBody] SendRealTimeMessageToUser request)
        {
            await _bus.Publish(request);
            return Ok();
        }


        [AllowAnonymous]
        [HttpPost("RealTime")]
        public async Task<ActionResult> Send([FromBody] SendRealTimeMessageToAddress request)
        {
            await _bus.Publish(request);
            return Ok();
        }



        [AllowAnonymous]
        [HttpPut("ReSendFailedSmsMessage")]
        public async Task<ActionResult> ReSendFailedSmsMessage([FromBody] ReSendFailedSmsMessageRequest request)
        {
            return Ok(await _mediator.Send(request));
        }


        //todo prevent external request
        [AllowAnonymous]
        [HttpPost("Verify")]
        public async Task<ActionResult> Verify([FromBody] SendVerifySmsRequest request)
        {
            await _bus.Publish(_mapper.Map<SendVerifySms>(request));
            return Ok();
        }


        [AllowAnonymous]
        [HttpPut("{id}/NotificationDelivered")]
        public async Task<ActionResult> NotificationDelivered(Guid id)
        {
            await _bus.Publish(new NotificationDelivered { Id = id });
            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("{id}/NotificationClicked")]
        public async Task<ActionResult> NotificationClicked(Guid id)
        {
            await _bus.Publish(new NotificationClicked { Id = id });
            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("NotificationEvent")]
        public async Task<ActionResult> NotificationEvent([FromBody] NotificationEventRequest request)
        {
            await _bus.Publish(request);
            if (request.EventName == "received")
                await _bus.Publish(new NotificationDelivered { Id = (Guid)request.MessageId, FCMMessageId = request.FCMMessageId });
            if (request.EventName == "opened" || request.EventName == "closed")
                await _bus.Publish(new NotificationClicked { Id = (Guid)request.MessageId, FCMMessageId = request.FCMMessageId });
            return Ok();
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _bus.Publish(new DeleteMessage { Id = id });
            return Ok();
        }

        [AllowAnonymous]
        [HttpDelete("Tag")]
        public async Task<ActionResult> DeleteByTag([FromBody] DeleteMessageByTagRequest request)
        {
            await _mediator.Send(new DeleteMessageByTagRequest { Tag = request.Tag, DeleteDirect = true });
            return Ok();
        }

        [HttpGet("My")]
        public async Task<ActionResult<List<MessageModel>>> ReadByUserId()
        {
            var result = await _mediator.Send(new ReadMessageByUserIdRequest { UserId = _identityService.UserId });
            return Ok(result);
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<MessageModel>> Read(Guid id)
        {
            var result = await _mediator.Send(new ReadMessageRequest { Id = id });
            return Ok(result);
        }
    }
}