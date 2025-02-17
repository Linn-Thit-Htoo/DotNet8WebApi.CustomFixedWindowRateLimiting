﻿namespace DotNet8WebApi.CustomFixedWindowRateLimiting.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RateLimitingController : ControllerBase
{
    private readonly FixedWindowRateLimiter _fixedWindowRateLimiter;

    public RateLimitingController(FixedWindowRateLimiter fixedWindowRateLimiter)
    {
        _fixedWindowRateLimiter = fixedWindowRateLimiter;
    }

    #region Execute

    [HttpPost("fixed-window")]
    public IActionResult Execute()
    {
        var context = HttpContext;
        var clientId = context.Connection.RemoteIpAddress?.ToString();

        if (!_fixedWindowRateLimiter.IsRequestAllowed(clientId!))
        {
            return StatusCode(429, "Too many requests.");
        }

        return Ok();
    }

    #endregion
}
