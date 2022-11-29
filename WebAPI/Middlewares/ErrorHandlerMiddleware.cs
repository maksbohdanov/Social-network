using BuisnessLogicLayer.Exceptions;
using WebAPI.Models;

namespace WebAPI.Middlewares
{
    public class ErrorHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                var error = new ErrorDetails() { Message = ex.Message };

                switch (ex)
                {
                    case NotFoundException:
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        break;

                    case SocialNetworkException:
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        break;

                    default:
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        error.Message = "Internal error occured. Please try later." + "\n" + ex.Message;
                        break;
                }

                error.Status = context.Response.StatusCode.ToString();                

                await context.Response.WriteAsync(error.ToString());
            }
        }

    }
}
