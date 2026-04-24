using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Application.DTO
{
    public class TemporalBlockRequest
    {
        [Required(ErrorMessage = "Country code is required")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Country code must be 2 letters")]
        public string CountryCode { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, 1440, ErrorMessage = "Duration must be between 1 and 1440 minutes")]
        public int DurationMinutes { get; set; }
    }
}
