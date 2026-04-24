using BlockedCountries.Application.DTO.ResultDTO;
using BlockedCountries.Application.Interfaces;
using BlockedCountries.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace BlockedCountries.Infrastructure.Services
{
    public class IpCheckService : IIpCheckService
    {
        private readonly IIpService _ipService;
        private readonly ICountryService _countryService;
        private readonly ILogService _logService;

        public IpCheckService(
            IIpService ipService,
            ICountryService countryService,
            ILogService logService)
        {
            _ipService = ipService;
            _countryService = countryService;
            _logService = logService;
        }

        public async Task<IpCheckResult> CheckAsync(HttpContext httpContext, string? ipAddress = null)
        {
            var forwardedIp = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            var remoteIp = httpContext.Connection.RemoteIpAddress?.ToString();

            string? finalIpAddress = ipAddress;

            if (string.IsNullOrWhiteSpace(finalIpAddress))
            {
                finalIpAddress = forwardedIp?.Split(',')[0].Trim() ?? remoteIp;
            }
             
            var ipInfo = await _ipService.LookupAsync(finalIpAddress, forwardedIp, remoteIp);

            var isBlocked = _countryService.IsCountryBlocked(ipInfo.CountryCode) ||
                            _countryService.IsTemporarilyBlocked(ipInfo.CountryCode);
            if (isBlocked)
            {
                var blockLog = new BlockLog
                {
                    IpAddress = ipInfo.Ip,
                    CountryCode = ipInfo.CountryCode,
                    Timestamp = DateTime.UtcNow,
                    IsBlocked = true,
                    UserAgent = httpContext.Request.Headers["User-Agent"].ToString(),
                    CheckedIp = finalIpAddress, 
                    BlockReason = _countryService.IsCountryBlocked(ipInfo.CountryCode)
                        ? "Permanently Blocked"
                        : "Temporarily Blocked"
                };

                _logService.Add(blockLog);
            }

            return new IpCheckResult
            {
                Ip = ipInfo.Ip,
                CountryCode = ipInfo.CountryCode,
                CountryName = ipInfo.CountryName,
                IsBlocked = isBlocked,
                BlockReason = isBlocked ? (_countryService.IsCountryBlocked(ipInfo.CountryCode)
                    ? "Country is permanently blocked"
                    : "Country is temporarily blocked") : null
            };
        }
    }
}