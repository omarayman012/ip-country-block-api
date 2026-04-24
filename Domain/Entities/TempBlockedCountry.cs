using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Domain.Entities
{
    public class TempBlockedCountry
    {
        public string CountryCode { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
