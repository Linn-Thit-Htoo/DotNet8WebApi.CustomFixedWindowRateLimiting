using DotNet8WebApi.CustomFixedWindowRateLimiting.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNet8WebApi.CustomFixedWindowRateLimiting.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RateLimitAttribute : Attribute, IAsyncActionFilter
    {
        public int Limit { get; }
        public int WindowSeconds { get; }

        public RateLimitAttribute(int limit, int windowSeconds)
        {
            Limit = limit;
            WindowSeconds = windowSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var clientId = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            var rateLimiter = (FixedWindowRateLimiter)context.HttpContext.RequestServices.GetService(typeof(FixedWindowRateLimiter))!;

            if (rateLimiter != null && !rateLimiter.IsRequestAllowed(clientId!))
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.");
                return;
            }

            await next();
        }
    }
}
