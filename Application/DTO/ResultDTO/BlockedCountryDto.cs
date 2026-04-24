using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Application.DTO.ResultDTO
{
    public class BlockedCountryDto
    {
        public string Code { get; set; } = null!;
        public string Type { get; set; } = null!; 
        public DateTime? Expiry { get; set; }
    }
}
