using BlockedCountries.Application.DTO.ResultDTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Application.Interfaces
{

    
        public interface IIpCheckService
        {
            Task<IpCheckResult> CheckAsync(HttpContext httpContext, string? ipAddress = null);
        }
    
}
