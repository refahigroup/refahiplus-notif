using AutoMapper;
using Refahi.Notif.Messages.NotifCenter;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Refahi.Notif.Application.Contract.Dtos.User.Commands;
using Refahi.Notif.Application.Contract.Services;
using Refahi.Notif.EndPoint.Api.V1.User.Dtos;

namespace Refahi.Notif.EndPoint.Api.V1.User
{
    public class UserController : BaseController
    {
        private readonly IBus _bus;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        public UserController(IBus bus, IMapper mapper, IMediator mediator, IIdentityService identityService)
        {
            _bus = bus;
            _mapper = mapper;
            _mediator = mediator;
            _identityService = identityService;
        }


        [HttpPut("UpsertDevice")]
        public async Task<ActionResult> UpsertDevice([FromBody] UpsertDeviceDto request)
        {
            request.DeviceId ??= _identityService.DeviceId;

            await _bus.Publish(new UpsertDevice
            {
                DeviceId = request.DeviceId.Value,
                UserId = _identityService.UserId,
                NotificationToken = request.NotificationToken,
                Type = request.Type,
                App = request.App,
            });
            return Ok();
        }


        [AllowAnonymous]
        [HttpPut("SetPhoneNumber")]
        public async Task<ActionResult> SetPhoneNumber([FromBody] SetUserPhoneNumberRequest request)
        {
            await _bus.Publish(_mapper.Map<SetUserPhoneNumberRequest>(request));
            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("SetEmail")]
        public async Task<ActionResult> SetEmail([FromBody] SetUserEmailRequest request)
        {
            await _bus.Publish(_mapper.Map<SetUserEmailRequest>(request));
            return Ok();
        }

    }
}