using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using WetterReiseApp.Models;

namespace WetterReiseApp.Services;

public sealed class WeatherTravelService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly Dictionary<int, (string Text, string Mark)> WeatherCodes = new()
    {
        [0] = ("Klarer Himmel", "S"),
        [1] = ("Meist sonnig", "S"),
        [2] = ("Teilweise bewölkt", "C"),
        [3] = ("Bewölkt", "C"),
        [45] = ("Nebel", "F"),
        [48] = ("Raureifnebel", "F"),
        [51] = ("Leichter Nieselregen", "R"),
        [53] = ("Nieselregen", "R"),
        [55] = ("Starker Nieselregen", "R"),
        [61] = ("Leichter Regen", "R"),
        [63] = ("Regen", "R"),
        [65] = ("Starker Regen", "R"),
        [71] = ("Leichter Schneefall", "N"),
        [73] = ("Schneefall", "N"),
        [75] = ("Starker Schneefall", "N"),
        [77] = ("Schneekörner", "N"),
        [80] = ("Regenschauer", "R"),
        [81] = ("Starke Regenschauer", "R"),
        [82] = ("Heftige Regenschauer", "R"),
        [85] = ("Schneeschauer", "N"),
        [86] = ("Starke Schneeschauer", "N"),
        [95] = ("Gewitter", "T"),
        [96] = ("Gewitter mit Hagel", "T"),
        [99] = ("Starkes Gewitter mit Hagel", "T")
    };

    private static readonly string[] AttractionBlocklist =
    [
        "bezirk",
        "gemeinde",
        "liste",
        "bahnhof",
        "haltestelle",
        "strasse",
        "straße",
        "autobahn",
        "bezirksteil"
    ];

    private readonly HttpClient _httpClient;

    public WeatherTravelService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("WetterReiseApp/1.0 Schulprojekt");
    }

    public async Task<WeatherTravelViewModel> GetForCityAsync(string? city, CancellationToken cancellationToken)
    {
        var place = string.IsNullOrWhiteSpace(city)
            ? new GeoPlace("Wien", "Österreich", 48.2085, 16.3721)
            : await SearchCityAsync(city, cancellationToken);

        return await GetForPlaceAsync(place, cancellationToken);
    }

    public async Task<WeatherTravelViewModel> GetForCoordinatesAsync(double latitude, double longitude, CancellationToken cancellationToken)
    {
        var place = new GeoPlace("Aktueller Standort", "", latitude, longitude);
        return await GetForPlaceAsync(place, cancellationToken);
    }

    private async Task<GeoPlace> SearchCityAsync(string city, CancellationToken cancellationToken)
    {
        var url = "https://geocoding-api.open-meteo.com/v1/search"
            + $"?name={Uri.EscapeDataString(city)}&count=1&language=de&format=json";
        var response = await GetFromJsonAsync<GeoCodingResponse>(url, cancellationToken);
        var result = response.Results?.FirstOrDefault();

        if (result is null)
        {
            throw new InvalidOperationException("Keine passende Stadt gefunden.");
        }

        return new GeoPlace(result.Name, result.Country ?? "", result.Latitude, result.Longitude);
    }

    private async Task<WeatherTravelViewModel> GetForPlaceAsync(GeoPlace place, CancellationToken cancellationToken)
    {
        var weatherTask = LoadWeatherAsync(place, cancellationToken);
        var attractionTask = LoadAttractionsAsync(place, cancellationToken);

        await Task.WhenAll(weatherTask, attractionTask);

        var weather = await weatherTask;
        var attractions = await attractionTask;
        var todayRain = weather.Daily.PrecipitationProbabilityMax.FirstOrDefault();
        var todayUv = weather.Daily.UvIndexMax.FirstOrDefault();
        var codeInfo = GetWeatherInfo(weather.Current.WeatherCode);
        var outfit = BuildOutfitAdvice(weather.Current.ApparentTemperature, todayRain, todayUv, weather.Current.WeatherCode, weather.Current.WindSpeed);
        var comfort = BuildComfort(weather.Current.ApparentTemperature, todayRain, todayUv, weather.Current.WindSpeed);

        return new WeatherTravelViewModel(
            place.Name,
            place.Country,
            place.Latitude,
            place.Longitude,
            DateTimeOffset.Parse(weather.Current.Time, CultureInfo.InvariantCulture),
            Round(weather.Current.Temperature),
            Round(weather.Current.ApparentTemperature),
            Round(weather.Current.Humidity),
            Round(todayRain),
            Round(weather.Current.WindSpeed),
            Round(weather.Current.WindGusts),
            Round(todayUv),
            Round(weather.Current.CloudCover),
            weather.Current.IsDay == 1,
            codeInfo.Text,
            codeInfo.Mark,
            comfort.Title,
            comfort.Text,
            Round(weather.Daily.TemperatureMax.FirstOrDefault()),
            Round(weather.Daily.TemperatureMin.FirstOrDefault()),
            DateTimeOffset.Parse(weather.Daily.Sunrise.First(), CultureInfo.InvariantCulture),
            DateTimeOffset.Parse(weather.Daily.Sunset.First(), CultureInfo.InvariantCulture),
            outfit,
            BuildHourlyForecasts(weather),
            BuildDailyForecasts(weather),
            attractions);
    }

    private async Task<OpenMeteoForecast> LoadWeatherAsync(GeoPlace place, CancellationToken cancellationToken)
    {
        var latitude = ToInvariant(place.Latitude);
        var longitude = ToInvariant(place.Longitude);
        var url = "https://api.open-meteo.com/v1/forecast"
            + $"?latitude={latitude}&longitude={longitude}"
            + "&current=temperature_2m,relative_humidity_2m,apparent_temperature,is_day,precipitation,weather_code,cloud_cover,wind_speed_10m,wind_gusts_10m"
            + "&hourly=temperature_2m,precipitation_probability,weather_code,wind_speed_10m"
            + "&daily=weather_code,temperature_2m_max,temperature_2m_min,precipitation_probability_max,sunrise,sunset,uv_index_max"
            + "&timezone=auto&forecast_days=7";

        return await GetFromJsonAsync<OpenMeteoForecast>(url, cancellationToken);
    }

    private async Task<IReadOnlyList<Attraction>> LoadAttractionsAsync(GeoPlace place, CancellationToken cancellationToken)
    {
        var latitude = ToInvariant(place.Latitude);
        var longitude = ToInvariant(place.Longitude);
        var url = "https://de.wikipedia.org/w/api.php"
            + "?action=query&generator=geosearch"
            + $"&ggscoord={latitude}%7C{longitude}&ggsradius=15000&ggslimit=12"
            + "&prop=pageimages%7Cextracts%7Ccoordinates&exintro=1&explaintext=1&exchars=170"
            + "&piprop=thumbnail&pithumbsize=600&format=json&origin=*";

        try
        {
            using var stream = await _httpClient.GetStreamAsync(url, cancellationToken);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

            if (!document.RootElement.TryGetProperty("query", out var query)
                || !query.TryGetProperty("pages", out var pages))
            {
                return [];
            }

            var attractions = new List<Attraction>();
            foreach (var page in pages.EnumerateObject().Select(property => property.Value))
            {
                var title = page.GetProperty("title").GetString() ?? "";
                if (AttractionBlocklist.Any(word => title.Contains(word, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                var pageId = page.GetProperty("pageid").GetInt32();
                var extract = page.TryGetProperty("extract", out var extractElement)
                    ? extractElement.GetString() ?? "Kurzbeschreibung ist auf Wikipedia verfügbar."
                    : "Kurzbeschreibung ist auf Wikipedia verfügbar.";
                var imageUrl = page.TryGetProperty("thumbnail", out var thumbnail)
                    && thumbnail.TryGetProperty("source", out var source)
                    ? source.GetString()
                    : null;

                attractions.Add(new Attraction(title, extract, imageUrl, $"https://de.wikipedia.org/?curid={pageId}"));
            }

            return attractions.Take(8).ToList();
        }
        catch
        {
            return [];
        }
    }

    private async Task<T> GetFromJsonAsync<T>(string url, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken);
        return data ?? throw new InvalidOperationException("API-Antwort konnte nicht gelesen werden.");
    }

    private static IReadOnlyList<HourlyForecast> BuildHourlyForecasts(OpenMeteoForecast weather)
    {
        var now = DateTimeOffset.Parse(weather.Current.Time, CultureInfo.InvariantCulture);
        var hours = new List<HourlyForecast>();

        for (var index = 0; index < weather.Hourly.Time.Count && hours.Count < 12; index++)
        {
            var time = DateTimeOffset.Parse(weather.Hourly.Time[index], CultureInfo.InvariantCulture);
            if (time < now)
            {
                continue;
            }

            var weatherInfo = GetWeatherInfo(weather.Hourly.WeatherCode[index]);
            hours.Add(new HourlyForecast(
                time,
                Round(weather.Hourly.Temperature[index]),
                Round(weather.Hourly.PrecipitationProbability[index]),
                weatherInfo.Mark));
        }

        return hours;
    }

    private static IReadOnlyList<DailyForecast> BuildDailyForecasts(OpenMeteoForecast weather)
    {
        var days = new List<DailyForecast>();
        for (var index = 0; index < weather.Daily.Time.Count; index++)
        {
            var weatherInfo = GetWeatherInfo(weather.Daily.WeatherCode[index]);
            days.Add(new DailyForecast(
                DateTimeOffset.Parse(weather.Daily.Time[index], CultureInfo.InvariantCulture),
                Round(weather.Daily.TemperatureMax[index]),
                Round(weather.Daily.TemperatureMin[index]),
                Round(weather.Daily.PrecipitationProbabilityMax[index]),
                weatherInfo.Text,
                weatherInfo.Mark));
        }

        return days;
    }

    private static IReadOnlyList<OutfitAdvice> BuildOutfitAdvice(double feelsLike, double rainChance, double uvIndex, int weatherCode, double windSpeed)
    {
        var items = new List<OutfitAdvice>();

        if (feelsLike <= 0)
        {
            items.Add(new("W", "Winterjacke", "Sehr kalt: dicke Jacke, Mütze und Handschuhe einplanen."));
        }
        else if (feelsLike <= 8)
        {
            items.Add(new("J", "Warme Jacke", "Kalt genug für Schal und eine isolierende Schicht."));
        }
        else if (feelsLike <= 15)
        {
            items.Add(new("L", "Zwiebellook", "Leichte Jacke plus Pullover, damit du flexibel bleibst."));
        }
        else if (feelsLike <= 22)
        {
            items.Add(new("M", "Leichte Schicht", "T-Shirt oder Hemd, dazu eine dünne Jacke für Schatten und Abend."));
        }
        else if (feelsLike <= 28)
        {
            items.Add(new("S", "Sommeroutfit", "Atmungsaktive Kleidung und bequeme Schuhe passen gut."));
        }
        else
        {
            items.Add(new("H", "Hitzetauglich", "Locker, hell, luftig: Wasserflasche und Pausen im Schatten."));
        }

        if (rainChance >= 45 || IsRainCode(weatherCode))
        {
            items.Add(new("R", "Regenschutz", "Schirm oder Regenjacke mitnehmen, Schuhe besser wasserfest wählen."));
        }

        if (IsSnowCode(weatherCode))
        {
            items.Add(new("N", "Schneefest", "Warme Socken, rutschfeste Schuhe und wasserabweisende Schicht."));
        }

        if (windSpeed >= 35)
        {
            items.Add(new("G", "Windbreaker", "Es wird windig: Kapuze oder Windjacke macht den Tag angenehmer."));
        }

        if (uvIndex >= 6)
        {
            items.Add(new("U", "Sonnenschutz", "Sonnenbrille, Kopfbedeckung und Sonnencreme sind sinnvoll."));
        }

        if (items.Count < 4)
        {
            items.Add(new("B", "Bequeme Schuhe", "Gut für Stadtspaziergang und Sehenswürdigkeiten in der Nähe."));
        }

        return items.Take(5).ToList();
    }

    private static (string Title, string Text) BuildComfort(double feelsLike, double rainChance, double uvIndex, double windSpeed)
    {
        var score = 100;
        if (feelsLike is < 4 or > 30) score -= 25;
        if (feelsLike is < -4 or > 34) score -= 15;
        if (rainChance > 35) score -= 18;
        if (rainChance > 65) score -= 15;
        if (windSpeed > 30) score -= 12;
        if (uvIndex > 7) score -= 8;

        return score switch
        {
            >= 78 => ("Sehr gut für draußen", "Das Wetter spielt mit. Plane Sehenswürdigkeiten, Spaziergang und längere Wege ruhig großzügig ein."),
            >= 58 => ("Gut mit etwas Planung", "Draußen ist gut machbar. Nimm die Kleidungsempfehlung ernst und halte eine Indoor-Option bereit."),
            >= 38 => ("Gemischter Tag", "Kurze Wege, Pausen und wetterfeste Kleidung sind heute die bessere Strategie."),
            _ => ("Eher Indoor-freundlich", "Das Wetter ist anspruchsvoll. Museen, Cafés oder kurze Attraktionsstopps sind heute angenehmer.")
        };
    }

    private static (string Text, string Mark) GetWeatherInfo(int code)
    {
        return WeatherCodes.GetValueOrDefault(code, ("Unbekanntes Wetter", "W"));
    }

    private static bool IsRainCode(int code)
    {
        return code is 51 or 53 or 55 or 61 or 63 or 65 or 80 or 81 or 82 or 95 or 96 or 99;
    }

    private static bool IsSnowCode(int code)
    {
        return code is 71 or 73 or 75 or 77 or 85 or 86;
    }

    private static int Round(double value)
    {
        return (int)Math.Round(value, MidpointRounding.AwayFromZero);
    }

    private static string ToInvariant(double value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    private sealed record GeoPlace(string Name, string Country, double Latitude, double Longitude);

    private sealed record GeoCodingResponse(IReadOnlyList<GeoResult>? Results);

    private sealed record GeoResult(
        string Name,
        string? Country,
        double Latitude,
        double Longitude);

    private sealed record OpenMeteoForecast(
        CurrentWeather Current,
        HourlyWeather Hourly,
        DailyWeather Daily);

    private sealed record CurrentWeather(
        string Time,
        [property: JsonPropertyName("temperature_2m")] double Temperature,
        [property: JsonPropertyName("relative_humidity_2m")] double Humidity,
        [property: JsonPropertyName("apparent_temperature")] double ApparentTemperature,
        [property: JsonPropertyName("is_day")] int IsDay,
        [property: JsonPropertyName("weather_code")] int WeatherCode,
        [property: JsonPropertyName("cloud_cover")] double CloudCover,
        [property: JsonPropertyName("wind_speed_10m")] double WindSpeed,
        [property: JsonPropertyName("wind_gusts_10m")] double WindGusts);

    private sealed record HourlyWeather(
        IReadOnlyList<string> Time,
        [property: JsonPropertyName("temperature_2m")] IReadOnlyList<double> Temperature,
        [property: JsonPropertyName("precipitation_probability")] IReadOnlyList<double> PrecipitationProbability,
        [property: JsonPropertyName("weather_code")] IReadOnlyList<int> WeatherCode);

    private sealed record DailyWeather(
        IReadOnlyList<string> Time,
        [property: JsonPropertyName("weather_code")] IReadOnlyList<int> WeatherCode,
        [property: JsonPropertyName("temperature_2m_max")] IReadOnlyList<double> TemperatureMax,
        [property: JsonPropertyName("temperature_2m_min")] IReadOnlyList<double> TemperatureMin,
        [property: JsonPropertyName("precipitation_probability_max")] IReadOnlyList<double> PrecipitationProbabilityMax,
        IReadOnlyList<string> Sunrise,
        IReadOnlyList<string> Sunset,
        [property: JsonPropertyName("uv_index_max")] IReadOnlyList<double> UvIndexMax);
}
