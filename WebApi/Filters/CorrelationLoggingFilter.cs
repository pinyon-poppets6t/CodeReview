namespace WebApi.Filters;

using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CorrelationLoggingFilterAttribute : Attribute, IAsyncActionFilter
{
    private const string CorrelationHeader = "X-Correlation-Id";

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var httpContext = context.HttpContext;
        var request = httpContext.Request;
        var response = httpContext.Response;

        var correlationId = request.Headers[CorrelationHeader].FirstOrDefault() ?? Guid.NewGuid().ToString();

        request.Headers[CorrelationHeader] = correlationId;
        response.Headers[CorrelationHeader] = correlationId;

        var logger = httpContext.RequestServices.GetRequiredService<ILogger<CorrelationLoggingFilterAttribute>>();

        var sw = Stopwatch.StartNew();

        logger.LogInformation($"Started {request.Method} {request.Path} (CorrelationId: {correlationId})");

        _ = await next();

        sw.Stop();

        logger.LogInformation(
            $"Finished {request.Method} {request.Path} with {response.StatusCode} in {sw.ElapsedMilliseconds} ms (CorrelationId: {correlationId})");
    }
}