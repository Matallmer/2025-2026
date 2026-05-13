namespace BlazorApp1.Models;

public sealed record LocationSearchResult(
    string Name,
    string Country,
    string CountryCode,
    string? AdminArea,
    double Latitude,
    double Longitude,
    string? Timezone)
{
    public string DisplayName =>
        string.IsNullOrWhiteSpace(AdminArea)
            ? $"{Name}, {Country}"
            : $"{Name}, {AdminArea}, {Country}";
}

public sealed class WeatherOverview
{
    public required string LocationName { get; init; }
    public required double CurrentTemperatureC { get; init; }
    public required int WeatherCode { get; init; }
    public required string WeatherDescription { get; init; }
    public required double WindSpeedKmH { get; init; }
    public required IReadOnlyList<DailyForecast> NextDays { get; init; }
}

public sealed record DailyForecast(
    DateOnly Date,
    double MaxTemperatureC,
    double MinTemperatureC,
    int WeatherCode,
    string WeatherDescription);

public sealed class CountryProfile
{
    public required string Name { get; init; }
    public string? Capital { get; init; }
    public required string Region { get; init; }
    public string? Subregion { get; init; }
    public long? Population { get; init; }
    public IReadOnlyList<string> Languages { get; init; } = [];
    public IReadOnlyList<string> Timezones { get; init; } = [];
    public string? Currency { get; init; }
    public string? FlagUrl { get; init; }
}

public sealed class FoodProductSummary
{
    public required string Code { get; init; }
    public required string Name { get; init; }
    public string? Brand { get; init; }
    public string? ImageUrl { get; init; }
    public string? NutriScore { get; init; }
}

public sealed class FoodProductDetails
{
    public required string Code { get; init; }
    public required string Name { get; init; }
    public string? Brand { get; init; }
    public string? ImageUrl { get; init; }
    public string? Countries { get; init; }
    public string? Ingredients { get; init; }
    public string? NutriScore { get; init; }
    public double? EnergyKcal100g { get; init; }
    public double? Fat100g { get; init; }
    public double? Carbohydrates100g { get; init; }
    public double? Sugars100g { get; init; }
    public double? Protein100g { get; init; }
    public double? Salt100g { get; init; }
}

public sealed class FavoriteItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Type { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string? CountryCode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Timezone { get; set; }
    public string? ProductCode { get; set; }
}
