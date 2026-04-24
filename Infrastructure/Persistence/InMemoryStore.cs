using BlockedCountries.Domain.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Infrastructure.Persistence
{
    public static class InMemoryStore
    {
        public static ConcurrentDictionary<string, Country> BlockedCountries { get; } = new();
        public static ConcurrentDictionary<string, DateTime> TemporalBlocks { get; } = new();

        public static ConcurrentBag<BlockLog> Logs { get; } = new();
    }
}
