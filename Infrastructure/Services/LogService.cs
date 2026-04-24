using BlockedCountries.Application.Common;
using BlockedCountries.Application.Interfaces;
using BlockedCountries.Domain.Entities;
using BlockedCountries.Infrastructure.Persistence;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LogService : ILogService
{
    public void Add(BlockLog log)
    {
        InMemoryStore.Logs.Add(log);
        Console.WriteLine($"[LOG] Added: {log.CountryCode} - {log.IpAddress} - Blocked: {log.IsBlocked}");
    }

    public PaginatedResult<BlockLog> GetBlockedAttempts(int page, int pageSize)
    {
        var query = InMemoryStore.Logs
            .Where(log => log.IsBlocked)  
            .OrderByDescending(log => log.Timestamp)
            .AsQueryable();

        var totalCount = query.Count();

        var data = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginatedResult<BlockLog>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Data = data
        };
    }
}


