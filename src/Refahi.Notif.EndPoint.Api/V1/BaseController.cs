using Refahi.Notif.EndPoint.Api.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Refahi.Notif.EndPoint.Api.V1
{
    [ApiVersion("1")]
    [Authorize]
    [ApiController]
    [ModelValidator]
    [Route("V1/[controller]")]
    public class BaseController : ControllerBase
    {
    }
}
