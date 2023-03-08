namespace FluxoDeCaixa.API;

using System.Net;
using System.Text.Json;

public class MiddlewareExceptionHandler : IMiddleware
{
    private readonly ILogger<MiddlewareExceptionHandler> _logger;

    public MiddlewareExceptionHandler(ILogger<MiddlewareExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var correlationId = context.TraceIdentifier;

            _logger.LogError(ex, "An error occurred. CorrelationId: {CorrelationId}", correlationId);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var errorResponse = new
            {
                error = ex.Message,
                correlationId
            };

            var errorJson = System.Text.Json.JsonSerializer.Serialize(errorResponse, options);

            await context.Response.WriteAsync(errorJson);
        }
    }
}
