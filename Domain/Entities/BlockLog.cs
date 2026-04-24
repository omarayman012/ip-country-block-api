using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Domain.Entities
{
    public class BlockLog
    {
        public int Id { get; set; } 
        public string IpAddress { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public bool IsBlocked { get; set; }
        public string UserAgent { get; set; } = string.Empty;
        public string? CheckedIp { get; set; } 
        public string? BlockReason { get; set; } 
    }
}
