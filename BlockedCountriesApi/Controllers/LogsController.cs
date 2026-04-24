using BlockedCountries.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountries.Api.Controllers
{
    [ApiController]
    [Route("api/logs")]
    public class LogsController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogsController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet("blocked-attempts")]
        public IActionResult GetBlockedAttempts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var result = _logService.GetBlockedAttempts(page, pageSize);
            return Ok(result);
        }
    }
}