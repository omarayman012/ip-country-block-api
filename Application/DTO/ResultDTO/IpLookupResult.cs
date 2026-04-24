using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Application.DTO.ResultDTO
{
    public class IpLookupResult
    {
        public string Ip { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string ISP { get; set; }
    }
}
