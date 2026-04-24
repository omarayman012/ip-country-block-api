using BlockedCountries.Application.Common;
using BlockedCountries.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Application.Interfaces
{
    public interface ILogService
    {
        void Add(BlockLog log);
        PaginatedResult<BlockLog> GetBlockedAttempts(int page, int pageSize);
    }
}
