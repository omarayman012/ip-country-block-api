using BlockedCountries.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlockedCountries.Api.Controllers
{
    [ApiController]
    [Route("api/ip")]
    public class IpController : ControllerBase
    {
        private readonly IIpService _ipService;
        private readonly IIpCheckService _ipCheckService;

        public IpController(
            IIpService ipService,
            IIpCheckService ipCheckService)
        {
            _ipService = ipService;
            _ipCheckService = ipCheckService;
        }

        [HttpGet("lookup")]
        public async Task<IActionResult> Lookup([FromQuery] string? ipAddress)
        {
            if (!string.IsNullOrWhiteSpace(ipAddress) && !IPAddress.TryParse(ipAddress, out _))
            {
                return BadRequest("Invalid IP format");
            }

            var forwardedIp = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            var result = await _ipService.LookupAsync(ipAddress, forwardedIp, remoteIp);

            return Ok(result);
        }
        [HttpGet("check-block")]
        public async Task<IActionResult> CheckBlock([FromQuery] string? ipAddress)
        {
            if (!string.IsNullOrWhiteSpace(ipAddress) && !IPAddress.TryParse(ipAddress, out _))
            {
                return BadRequest("Invalid IP format");
            }

            var result = await _ipCheckService.CheckAsync(HttpContext, ipAddress);
            return Ok(result);
        }
    }
}