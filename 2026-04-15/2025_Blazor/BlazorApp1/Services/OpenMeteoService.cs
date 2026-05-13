using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using BlazorApp1.Models;

namespace BlazorApp1.Services;

public sealed class OpenMeteoService(IHttpClientFactory httpClientFactory)
{
    public async Task<IReadOnlyList<LocationSearchResult>> SearchLocationsAsync(
        string query,
        int count = 8,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return [];
        }

        var client = httpClientFactory.CreateClient();
        var url =
            $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(query)}&count={count}&language=de&format=json";

        var payload = await client.GetFromJsonAsync<GeocodingResponse>(url, cancellationToken);

        if (payload?.Results is null)
        {
            return [];
        }

        return payload.Results
            .Where(x => x is not null && !string.IsNullOrWhiteSpace(x.Name) && !string.IsNullOrWhiteSpace(x.CountryCode))
            .Select(x => new LocationSearchResult(
                x!.Name!,
                x.Country ?? x.CountryCode!,
                x.CountryCode!,
                x.Admin1,
                x.Latitude,
                x.Longitude,
                x.Timezone))
            .ToList();
    }

    public async Task<WeatherOverview?> GetWeatherAsync(
        LocationSearchResult location,
        CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient();
        var latitude = location.Latitude.ToString(CultureInfo.InvariantCulture);
        var longitude = location.Longitude.ToString(CultureInfo.InvariantCulture);
        var url =
            $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current=temperature_2m,weather_code,wind_speed_10m&daily=weather_code,temperature_2m_max,temperature_2m_min&forecast_days=4&timezone=auto";

        var payload = await client.GetFromJsonAsync<ForecastResponse>(url, cancellationToken);

        if (payload?.Current is null || payload.Daily is null)
        {
            return null;
        }

        return new WeatherOverview
        {
            LocationName = location.DisplayName,
            CurrentTemperatureC = payload.Current.Temperature2m,
            WeatherCode = payload.Current.WeatherCode,
            WeatherDescription = DescribeWeatherCode(payload.Current.WeatherCode),
            WindSpeedKmH = payload.Current.WindSpeed10m,
            NextDays = BuildDailyForecast(payload.Daily)
        };
    }

    public static string DescribeWeatherCode(int weatherCode) =>
        weatherCode switch
        {
            0 => "Klarer Himmel",
            1 => "Ueberwiegend klar",
            2 => "Teilweise bewoelkt",
            3 => "Bedeckt",
            45 => "Nebel",
            48 => "Reifnebel",
            51 => "Leichter Nieselregen",
            53 => "Maessiger Nieselregen",
            55 => "Starker Nieselregen",
            61 => "Leichter Regen",
            63 => "Maessiger Regen",
            65 => "Starker Regen",
            71 => "Leichter Schneefall",
            73 => "Maessiger Schneefall",
            75 => "Starker Schneefall",
            80 => "Regenschauer",
            81 => "Maessige Regenschauer",
            82 => "Heftige Regenschauer",
            95 => "Gewitter",
            96 => "Gewitter mit leichtem Hagel",
            99 => "Gewitter mit starkem Hagel",
            _ => "Unbekannt"
        };

    private static IReadOnlyList<DailyForecast> BuildDailyForecast(DailyBlock daily)
    {
        if (daily.Time is null ||
            daily.WeatherCodes is null ||
            daily.MaxTemperatures is null ||
            daily.MinTemperatures is null)
        {
            return [];
        }

        var availableRows = new[]
        {
            daily.Time.Count,
            daily.WeatherCodes.Count,
            daily.MaxTemperatures.Count,
            daily.MinTemperatures.Count
        }.Min();

        var forecasts = new List<DailyForecast>();

        for (var i = 1; i < availableRows; i++)
        {
            if (!DateOnly.TryParse(daily.Time[i], out var day))
            {
                continue;
            }

            var code = daily.WeatherCodes[i];
            forecasts.Add(new DailyForecast(
                day,
                daily.MaxTemperatures[i],
                daily.MinTemperatures[i],
                code,
                DescribeWeatherCode(code)));
        }

        return forecasts;
    }

    private sealed class GeocodingResponse
    {
        [JsonPropertyName("results")]
        public List<GeocodingResult?>? Results { get; set; }
    }

    private sealed class GeocodingResult
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("country_code")]
        public string? CountryCode { get; set; }

        [JsonPropertyName("admin1")]
        public string? Admin1 { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("timezone")]
        public string? Timezone { get; set; }
    }

    private sealed class ForecastResponse
    {
        [JsonPropertyName("current")]
        public CurrentBlock? Current { get; set; }

        [JsonPropertyName("daily")]
        public DailyBlock? Daily { get; set; }
    }

    private sealed class CurrentBlock
    {
        [JsonPropertyName("temperature_2m")]
        public double Temperature2m { get; set; }

        [JsonPropertyName("weather_code")]
        public int WeatherCode { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public double WindSpeed10m { get; set; }
    }

    private sealed class DailyBlock
    {
        [JsonPropertyName("time")]
        public List<string>? Time { get; set; }

        [JsonPropertyName("weather_code")]
        public List<int>? WeatherCodes { get; set; }

        [JsonPropertyName("temperature_2m_max")]
        public List<double>? MaxTemperatures { get; set; }

        [JsonPropertyName("temperature_2m_min")]
        public List<double>? MinTemperatures { get; set; }
    }
}
