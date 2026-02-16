using Refahi.Notif.Domain.Core.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Refahi.Notif.EndPoint.Api.Filters
{

    public class ModelValidatorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(f => f.Errors.Select(g => g.ErrorMessage));

                throw new BussinessException(errors);
            }
        }
    }

}
