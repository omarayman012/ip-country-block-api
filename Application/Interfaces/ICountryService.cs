using BlockedCountries.Application.Common;
using BlockedCountries.Application.DTO;
using BlockedCountries.Application.DTO.ResultDTO;
using BlockedCountries.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Application.Interfaces
{
    public interface ICountryService
    {
 
        AddCountryResult AddBlockedCountry(string countryCode);
        bool RemoveBlockedCountry(string countryCode);
        PaginatedResult<BlockedCountryDto> GetAllBlocks(int page, int pageSize, string? search); bool IsCountryBlocked(string countryCode);

        bool AddTemporalBlock(string countryCode, int durationMinutes);
        bool IsTemporarilyBlocked(string countryCode);
        void RemoveExpiredTemporalBlocks();
    }
}