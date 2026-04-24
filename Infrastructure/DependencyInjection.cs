using BlockedCountries.Application.Interfaces;
using BlockedCountries.Infrastructure.BackgroundServices;
using BlockedCountries.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlockedCountries.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IIpService, IpService>();
            services.AddScoped<IIpCheckService, IpCheckService>();  
            services.AddScoped<ILogService, LogService>();  

            services.AddHostedService<TemporalBlockCleanupService>();

            services.AddHttpClient<IIpService, IpService>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(10);
                client.DefaultRequestHeaders.Add("User-Agent", "BlockedCountriesApi/1.0");
            });

            return services;
        }
    }
}