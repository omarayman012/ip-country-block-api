using BlockedCountries.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Application.DTO
{
    public class AddCountryResult
    {
        public bool Success { get; set; }
        public AddCountryError Error { get; set; }
    }
}
