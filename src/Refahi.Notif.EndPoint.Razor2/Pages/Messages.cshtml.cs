using Refahi.Notif.Application.Contract.Dtos.Message.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Refahi.Notif.Domain.Contract.Models;

namespace Refahi.Notif.EndPoint.Razor.Pages
{
    public class MessagesModel : PageModel
    {
        private readonly ILogger<MessagesModel> _logger;
        private readonly IMediator _mediator;
        public List<MessageModel> List;
        public MessagesModel(ILogger<MessagesModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task OnGet()
        {
            List = await _mediator.Send(new ReadMessageListRequest
            {
                PhoneNumber = Request.Query["phonenumber"].ToString()
            });
        }
    }
}