using System.Net;

namespace DotNet8WebApi.CustomFixedWindowRateLimiting.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RateLimitingController : ControllerBase
{
    private readonly FixedWindowRateLimiter _fixedWindowRateLimiter;
    private readonly HttpClient _httpClient;


    public RateLimitingController(FixedWindowRateLimiter fixedWindowRateLimiter, HttpClient httpClient)
    {
        _fixedWindowRateLimiter = fixedWindowRateLimiter;
        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri("https://localhost:7218");
    }

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
    [HttpGet]
    public async Task<IActionResult> GetBlogs()
    {
        //HttpResponseMessage response = await _httpClient.PostAsync("/api/RateLimiting/fixed-window", null);
        //var responseJson = await response.Content.ReadAsStringAsync();
        //var statusCode = response.StatusCode;

        //if(statusCode == HttpStatusCode.TooManyRequests)
        //{
        //    return StatusCode(429,responseJson);
        //}
        //return Ok();

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsync("/api/RateLimiting/fixed-window", null);
        }
        catch (HttpRequestException e)
        {
            return StatusCode(500, $"Internal server error: {e.Message}");
        }

        var responseJson = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            return StatusCode(429, responseJson);
        }

        return Ok(responseJson);
    }
}
