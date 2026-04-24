using BlockedCountries.Application.DTO.ResultDTO;
using BlockedCountries.Application.Interfaces;
using System.Text.Json;

public class IpService : IIpService
{
    private readonly HttpClient _httpClient;

    public IpService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IpLookupResult> LookupAsync(string? ipAddress, string? forwardedIp = null, string? remoteIp = null)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
        {
            ipAddress = forwardedIp?.Split(',')[0].Trim() ?? remoteIp;
        }

        if (string.IsNullOrWhiteSpace(ipAddress))
        {
            return new IpLookupResult
            {
                Ip = "UNKNOWN",
                CountryCode = "UNKNOWN",
                CountryName = "UNKNOWN",
                ISP = "UNKNOWN"
            };
        }

        var url = $"http://ip-api.com/json/{ipAddress}?fields=status,country,countryCode,isp,query";


        try
        {
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<JsonElement>(content);

                if (data.TryGetProperty("status", out var status) && status.GetString() == "success")
                {
                    return new IpLookupResult
                    {
                        Ip = ipAddress,
                        CountryCode = data.TryGetProperty("countryCode", out var countryCode)
                            ? countryCode.GetString() ?? "UNKNOWN"
                            : "UNKNOWN",
                        CountryName = data.TryGetProperty("country", out var countryName)
                            ? countryName.GetString() ?? "UNKNOWN"
                            : "UNKNOWN",
                        ISP = data.TryGetProperty("isp", out var isp)
                            ? isp.GetString() ?? "UNKNOWN"
                            : "UNKNOWN"
                    };
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error with ip-api.com: {ex.Message}");
        }

        try
        {
            var backupUrl = $"https://ipapi.co/{ipAddress}/json/";
            var backupResponse = await _httpClient.GetAsync(backupUrl);

            if (backupResponse.IsSuccessStatusCode)
            {
                var backupContent = await backupResponse.Content.ReadAsStringAsync();
                var backupData = JsonSerializer.Deserialize<JsonElement>(backupContent);

                if (backupData.TryGetProperty("error", out var error) && !error.GetBoolean())
                {
                    return new IpLookupResult
                    {
                        Ip = ipAddress,
                        CountryCode = backupData.TryGetProperty("country_code", out var countryCode)
                            ? countryCode.GetString() ?? "UNKNOWN"
                            : "UNKNOWN",
                        CountryName = backupData.TryGetProperty("country_name", out var countryName)
                            ? countryName.GetString() ?? "UNKNOWN"
                            : "UNKNOWN",
                        ISP = backupData.TryGetProperty("org", out var org)
                            ? org.GetString() ?? "UNKNOWN"
                            : "UNKNOWN"
                    };
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error with ipapi.co: {ex.Message}");
        }

        return new IpLookupResult
        {
            Ip = ipAddress,
            CountryCode = "UNKNOWN",
            CountryName = "UNKNOWN",
            ISP = "UNKNOWN"
        };
    }
}