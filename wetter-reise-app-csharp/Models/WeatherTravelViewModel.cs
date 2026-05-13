namespace WetterReiseApp.Models;

public sealed record WeatherTravelViewModel(
    string City,
    string Country,
    double Latitude,
    double Longitude,
    DateTimeOffset CurrentTime,
    int Temperature,
    int FeelsLike,
    int Humidity,
    int RainChance,
    int WindSpeed,
    int WindGusts,
    int UvIndex,
    int CloudCover,
    bool IsDay,
    string WeatherText,
    string WeatherMark,
    string ComfortTitle,
    string ComfortText,
    int TodayMax,
    int TodayMin,
    DateTimeOffset Sunrise,
    DateTimeOffset Sunset,
    IReadOnlyList<OutfitAdvice> Outfit,
    IReadOnlyList<HourlyForecast> Hourly,
    IReadOnlyList<DailyForecast> Daily,
    IReadOnlyList<Attraction> Attractions);

public sealed record OutfitAdvice(string Symbol, string Title, string Text);

public sealed record HourlyForecast(DateTimeOffset Time, int Temperature, int RainChance, string WeatherMark);

public sealed record DailyForecast(DateTimeOffset Date, int MaxTemperature, int MinTemperature, int RainChance, string WeatherText, string WeatherMark);

public sealed record Attraction(string Title, string Extract, string? ImageUrl, string Url);
