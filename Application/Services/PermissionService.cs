using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using Application.Interfaces;

namespace Application.Services;

public class PermissionService : IPermissionService
{
    private const string CorrelationHeader = "X-Correlation-Id";

    public async Task<bool> IsUserNotAllowed(
        string userId,
        HttpContext httpContext,
        CancellationToken ctx)
    {
        var request = httpContext.Request;
        var correlationId = request.Headers[CorrelationHeader].FirstOrDefault() ?? Guid.NewGuid().ToString();

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri($"https://localhost/permissions")
        };

        httpClient.DefaultRequestHeaders.Add(CorrelationHeader, correlationId);

        var response = await httpClient.GetAsync($"/api/permissions/user/{userId}", ctx);

        if (!response.IsSuccessStatusCode)
        {
            throw new IOException("External service has responded with unsuccessful response");
        }

        var permissionResponse =
            await response.Content.ReadFromJsonAsync<PermissionUserResponse>(cancellationToken: ctx);

        return !permissionResponse?.IsAllowed ?? true;
    }

    public record PermissionUserResponse(bool IsAllowed);
}