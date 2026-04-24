using BlockedCountries.Application.DTO;
using BlockedCountries.Application.Interfaces;
using BlockedCountries.Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountriesControllers.Controllers
{
    [ApiController]
    [Route("api/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

     
        [HttpPost("Create")]
        public IActionResult BlockCountry([FromBody] BlockCountryRequest request)
        {
            var result = _countryService.AddBlockedCountry(request.CountryCode);

            if (!result.Success)
            {
                return result.Error switch
                {
                    AddCountryError.InvalidCode => BadRequest("CountryCode must be 2 English letters (e.g., US, EG)"),
                    AddCountryError.DuplicateCode => Conflict("Country code already exists"),
                    _ => BadRequest()
                };
            }

            return Ok(new { message = "Country blocked successfully" });
        }
       
        [HttpDelete("Delete/{countryCode}")]
        public IActionResult UnblockCountry(string countryCode)
        {
            var result = _countryService.RemoveBlockedCountry(countryCode);

            if (!result)
                return NotFound("Country not found");

            return Ok(new { message = "Country unblocked successfully" });
        }

        [HttpPost("temporal-block")]
        public IActionResult AddTemporalBlock([FromBody] TemporalBlockRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _countryService.AddTemporalBlock(request.CountryCode, request.DurationMinutes);

            if (!result)
                return Conflict("Country already temporarily blocked or permanently blocked");

            return Ok(new { message = "Country temporarily blocked successfully" });
        }

        [HttpGet("all-blocked")]
        public IActionResult GetAllBlocks(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            var result = _countryService.GetAllBlocks(page, pageSize, search);
            return Ok(result);
        }
    }
}