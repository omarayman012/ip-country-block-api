using BlockedCountries.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlockedCountries.Infrastructure.BackgroundServices
{
    public class TemporalBlockCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TemporalBlockCleanupService> _logger;

        public TemporalBlockCleanupService(
            IServiceScopeFactory scopeFactory,
            ILogger<TemporalBlockCleanupService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TemporalBlockCleanupService started. Will run every 10 seconds.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var countryService = scope.ServiceProvider.GetRequiredService<ICountryService>();

                        await Task.Run(() => countryService.RemoveExpiredTemporalBlocks(), stoppingToken);

                        _logger.LogDebug("Temporal blocks cleanup executed at {Time}", DateTime.UtcNow);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while cleaning temporal blocks");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }

            _logger.LogInformation("TemporalBlockCleanupService is stopping.");
        }
    }
}