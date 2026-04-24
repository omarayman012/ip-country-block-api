using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Application.DTO.ResultDTO
{
    public class IpCheckResult
    {
        public string Ip { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public bool IsBlocked { get; set; }
        public string? BlockReason { get; set; }
    }
}
