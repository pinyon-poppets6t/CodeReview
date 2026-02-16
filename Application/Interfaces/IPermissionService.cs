using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IPermissionService
{
    public Task<bool> IsUserNotAllowed(string userId, HttpContext httpContext, CancellationToken ctx);
}