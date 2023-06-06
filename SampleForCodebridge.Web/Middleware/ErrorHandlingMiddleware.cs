using System.Net;
using FluentValidation;
using Newtonsoft.Json;

namespace SampleForCodebridge.Web.Middleware;

public class ErrorHandlingMiddleware
{
	private readonly RequestDelegate _next;

	public ErrorHandlingMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(context, ex);
		}
	}

	private static Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		HttpStatusCode statusCode;
		string message;

		switch (exception)
		{
			case FileNotFoundException:
				statusCode = HttpStatusCode.NotFound;
				message = exception.Message;
				break;
			
			case ArgumentException:
				statusCode = HttpStatusCode.Conflict;
				message = exception.Message;
				break;
			
			case ValidationException:
				statusCode = HttpStatusCode.BadRequest;
				message = exception.Message;
				break;

			default:
				statusCode = HttpStatusCode.InternalServerError;
				message = "Internal Server Error. " + exception.Message;
				break;
		}
		
		var result = JsonConvert.SerializeObject(new { error = message });
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)statusCode;
		return context.Response.WriteAsync(result);
	}
}