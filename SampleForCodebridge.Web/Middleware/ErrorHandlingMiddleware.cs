using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
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

			// case ValidationException validationException when validationException.ErrorCode == "InvalidInput":
			// 	statusCode = HttpStatusCode.BadRequest;
			// 	message = "Invalid input: " + validationException.Message;
			// 	break;
			//
			// case ValidationException validationException when validationException.ErrorCode == "AccessDenied":
			// 	statusCode = HttpStatusCode.Forbidden;
			// 	message = "Access denied: " + validationException.Message;
			// 	break;

			default:
				statusCode = HttpStatusCode.InternalServerError;
				message = "Internal Server Error";
				break;
		}


		var result = JsonConvert.SerializeObject(new { error = message });
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)statusCode;
		return context.Response.WriteAsync(result);
	}
}