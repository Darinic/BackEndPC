using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Thoughts.API.Middleware
{
	public class HttpContextMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public HttpContextMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
		{
			_next = next;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			_httpContextAccessor.HttpContext = context;

			await _next(context);
		}
	}

}
