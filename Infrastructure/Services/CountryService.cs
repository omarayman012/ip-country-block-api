using BlockedCountries.Application.Common;
using BlockedCountries.Application.DTO;
using BlockedCountries.Application.DTO.ResultDTO;
using BlockedCountries.Application.Interfaces;
using BlockedCountries.Domain.Entities;
using BlockedCountries.Domain.Enum;
using BlockedCountries.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Infrastructure.Services
{
    public class CountryService : ICountryService
    {
    

        public AddCountryResult AddBlockedCountry(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode) )
            {
                return new AddCountryResult
                {
                    Success = false,
                    Error = AddCountryError.InvalidCode
                };
            }

            countryCode = countryCode.ToUpper();

            if (countryCode.Length != 2 || !countryCode.All(char.IsLetter))
            {
                return new AddCountryResult
                {
                    Success = false,
                    Error = AddCountryError.InvalidCode
                };
            }

            if (InMemoryStore.BlockedCountries.ContainsKey(countryCode))
            {
                return new AddCountryResult
                {
                    Success = false,
                    Error = AddCountryError.DuplicateCode
                };
            }

           

            var country = new Country
            {
                Code = countryCode
            };

            InMemoryStore.BlockedCountries.TryAdd(countryCode, country);

            return new AddCountryResult
            {
                Success = true,
                Error = AddCountryError.None
            };
        }
        public bool RemoveBlockedCountry(string countryCode)
        {
            countryCode = countryCode.ToUpper();
            return InMemoryStore.BlockedCountries.TryRemove(countryCode, out _);
        }

      
        public bool IsCountryBlocked(string countryCode)
        {
            countryCode = countryCode.ToUpper();
            return InMemoryStore.BlockedCountries.ContainsKey(countryCode);
        }

        public bool AddTemporalBlock(string countryCode, int durationMinutes)
        {
            if (durationMinutes < 1 || durationMinutes > 1440)
                return false;

            countryCode = countryCode.ToUpper();

            if (countryCode.Length != 2 || !countryCode.All(char.IsLetter))
                return false;

            if (InMemoryStore.BlockedCountries.ContainsKey(countryCode))
                return false;

            if (InMemoryStore.TemporalBlocks.ContainsKey(countryCode))
                return false;

            var expiry = DateTime.UtcNow.AddMinutes(durationMinutes);
            return InMemoryStore.TemporalBlocks.TryAdd(countryCode, expiry);
        }

        public bool IsTemporarilyBlocked(string countryCode)
        {
            countryCode = countryCode.ToUpper();

            if (InMemoryStore.TemporalBlocks.TryGetValue(countryCode, out var expiry))
            {
                return expiry > DateTime.UtcNow;
            }

            return false;
        }

        public void RemoveExpiredTemporalBlocks()
        {
            var now = DateTime.UtcNow;

            var expired = InMemoryStore.TemporalBlocks
                .Where(x => x.Value <= now)
                .Select(x => x.Key)
                .ToList();

            foreach (var key in expired)
            {
                InMemoryStore.TemporalBlocks.TryRemove(key, out _);
            }
        }
        public PaginatedResult<BlockedCountryDto> GetAllBlocks(int page, int pageSize, string? search)
        {
            var permanent = InMemoryStore.BlockedCountries.Values
                .Select(c => new BlockedCountryDto
                {
                    Code = c.Code,
                    Type = "Permanent",
                    Expiry = null
                });

            var temporal = InMemoryStore.TemporalBlocks
                .Select(t => new BlockedCountryDto
                {
                    Code = t.Key,
                    Type = "Temporal",
                    Expiry = t.Value
                });

            var query = permanent.Concat(temporal);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.Code.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = query.Count();

            var data = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResult<BlockedCountryDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Data = data
            };
        }

    }
}
