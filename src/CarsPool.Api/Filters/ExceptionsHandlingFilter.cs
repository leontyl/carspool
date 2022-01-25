using CarsPool.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace CarsPool.Api.Filters
{
    public class ExceptionsHandlingFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            ObjectResult result = null;
            if(context.Exception != null)
            {
                if (context.Exception is CarsPoolDalException dalException)
                {
                    switch (dalException.ErrorCode)
                    {
                        case DalErrorCodes.EntityNotExists:
                            result = new ObjectResult(new { Error = dalException.Message })
                            {
                                StatusCode = (int?)HttpStatusCode.NotFound
                            };
                            break;
                        case DalErrorCodes.CarBelongsToOtherDriver:
                            result = new ObjectResult(new { Error = dalException.Message })
                            {
                                StatusCode = (int?)HttpStatusCode.Conflict
                            };
                            break;
                    }

                    if (result == null)
                    {
                        result = new ObjectResult(new { Error = "Internal server error." })
                        {
                            StatusCode = (int?)HttpStatusCode.InternalServerError
                        };
                    }

                    context.Result = result;
                    context.ExceptionHandled = true;
                }
            }
        }
    }
}
