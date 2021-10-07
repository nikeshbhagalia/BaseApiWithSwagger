using Base.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Base.Filters
{
    public class ExceptionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null || context.ExceptionHandled)
            {
                return;
            }

            context.ExceptionHandled = true;

            switch (context.Exception)
            {
                case BadRequestException badRequestException:
                    context.Result = new BadRequestObjectResult(badRequestException.Message?.ToString() ?? string.Empty);
                    return;

                case NotFoundException notFoundException:
                    context.Result = new NotFoundObjectResult(notFoundException.Message?.ToString() ?? string.Empty);
                    return;

                case ForbiddenException forbiddenException:
                    context.Result = new ForbidResult();
                    return;

                default:
                    context.Result = new StatusCodeResult((int)HttpStatusCode.InternalServerError);
                    return;
            }
        }
    }
}
