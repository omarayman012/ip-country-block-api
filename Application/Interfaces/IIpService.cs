using BlockedCountries.Application.DTO.ResultDTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BlockedCountries.Application.Interfaces
{

    public interface IIpService
    {
        Task<IpLookupResult> LookupAsync(string? ipAddress, string? forwardedIp = null, string? remoteIp = null);
    }
}
