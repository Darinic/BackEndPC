using Logistics.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net;
using Thoughts.Domain.Exceptions;
using ArgumentException = Thoughts.Domain.Exceptions.ArgumentException;

namespace Logistics.API.ExceptionHandling
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            await HandleExceptionAsync(context);
        }

        private Task HandleExceptionAsync(ExceptionContext context)
        {
            var exceptionResponse = HandleException(context.Exception);
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = exceptionResponse.StatusCode;
            context.ExceptionHandled = true;

            return context.HttpContext.Response.WriteAsync(exceptionResponse.ToString());
        }

        private static readonly Dictionary<Type, HttpStatusCode> ExceptionTypeToHttpStatusCode = new Dictionary<Type, HttpStatusCode>
		{
	        { typeof(ValidationException), HttpStatusCode.BadRequest },
	        { typeof(ConflictException), HttpStatusCode.Conflict },
	        { typeof(ArgumentException), HttpStatusCode.BadRequest },
	        { typeof(ServerSideException), HttpStatusCode.InternalServerError },
	        { typeof(UnauthorizedException), HttpStatusCode.Unauthorized },
        };


		public ExceptionResponse HandleException(Exception exception)
		{
			
			HttpStatusCode httpStatusCode;
			if (!ExceptionTypeToHttpStatusCode.TryGetValue(exception.GetType(), out httpStatusCode))
			{
				httpStatusCode = HttpStatusCode.InternalServerError;
				return new ExceptionResponse
				{
					StatusCode = (int)httpStatusCode,
					Message = "Internal Server Error, try again later"
				};
			}

			var exceptionResponse = new ExceptionResponse
			{
				StatusCode = (int)httpStatusCode,
				Message = exception.Message
			};

			return exceptionResponse;
		}
    }
}
