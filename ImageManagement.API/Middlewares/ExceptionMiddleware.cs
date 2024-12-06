namespace ImageManagement.API.Middlewares;

public class ExceptionMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ExceptionMiddleware> _logger;

	public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context); // Proceed to the next middleware
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An unexpected error occurred.");
			await HandleExceptionAsync(context, ex);
		}
	}

	private static Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		var statusCode = exception switch
		{
			KeyNotFoundException => (int)HttpStatusCode.NotFound, // 404
			UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized, // 401
			_ => (int)HttpStatusCode.InternalServerError // 500
		};

		var response = new
		{
			StatusCode = statusCode,
			Message = exception.Message,
			Details = exception.InnerException?.Message
		};

		context.Response.ContentType = "application/json";
		context.Response.StatusCode = statusCode;

		return context.Response.WriteAsync(JsonSerializer.Serialize(response));
	}
}
