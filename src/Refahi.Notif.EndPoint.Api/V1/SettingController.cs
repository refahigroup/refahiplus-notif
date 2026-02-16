using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Refahi.Notif.Application.Contract.Dtos.Setting.Command;
using Refahi.Notif.Application.Contract.Dtos.Setting.Queries;
using Refahi.Notif.Application.Contract.Services;

namespace Refahi.Notif.EndPoint.Api.V1
{
    public class SettingController : BaseController
    {
        private readonly IBus _bus;
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        public SettingController(IBus bus, IMediator mediator, IIdentityService identityService, IMapper mapper)
        {
            _bus = bus;
            _mediator = mediator;
            _identityService = identityService;
            _mapper = mapper;
        }

        //todo prevent external request
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetByKeyRequest request)
        {
            return Ok(await _mediator.Send(request));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Set([FromBody] UpdateSettingValueRequest request)
        {
            return Ok(await _mediator.Send(request));
        }

    }
}