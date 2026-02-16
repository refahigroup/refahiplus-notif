using Refahi.Notif.Application.Contract.Dtos.VerifySms.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Refahi.Notif.Domain.Contract.Models;

namespace Refahi.Notif.EndPoint.Razor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;
        public List<VerifyMessageModel> List;
        public IndexModel(ILogger<IndexModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task OnGet()
        {
            List = await _mediator.Send(new ReadVerifyMessageListRequest
            {
                PhoneNumber = Request.Query["phonenumber"].ToString()
            });
        }
    }
}