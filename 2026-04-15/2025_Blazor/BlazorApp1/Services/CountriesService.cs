using System.Text.Json;
using BlazorApp1.Models;

namespace BlazorApp1.Services;

public sealed class CountriesService(IHttpClientFactory httpClientFactory)
{
    public async Task<CountryProfile?> GetCountryProfileAsync(
        string countryCode,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return null;
        }

        var client = httpClientFactory.CreateClient();
        var safeCode = Uri.EscapeDataString(countryCode.Trim());
        var url =
            $"https://restcountries.com/v3.1/alpha/{safeCode}?fields=name,capital,region,subregion,population,languages,flags,currencies,timezones";

        using var response = await client.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var json = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

        JsonElement root = json.RootElement;
        if (root.ValueKind == JsonValueKind.Array)
        {
            if (root.GetArrayLength() == 0)
            {
                return null;
            }

            root = root[0];
        }

        var name = root.TryGetProperty("name", out var nameNode) && nameNode.TryGetProperty("common", out var commonNameNode)
            ? commonNameNode.GetString()
            : null;

        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var capital = TryGetFirstStringFromArray(root, "capital");
        var region = TryGetString(root, "region") ?? "Unbekannt";
        var subregion = TryGetString(root, "subregion");
        var population = TryGetLong(root, "population");
        var languages = TryGetObjectValues(root, "languages");
        var timezones = TryGetStringArray(root, "timezones");
        var currency = TryGetFirstCurrency(root);
        var flagUrl = TryGetFlagUrl(root);

        return new CountryProfile
        {
            Name = name,
            Capital = capital,
            Region = region,
            Subregion = subregion,
            Population = population,
            Languages = languages,
            Timezones = timezones,
            Currency = currency,
            FlagUrl = flagUrl
        };
    }

    private static string? TryGetString(JsonElement root, string propertyName) =>
        root.TryGetProperty(propertyName, out var node) && node.ValueKind == JsonValueKind.String
            ? node.GetString()
            : null;

    private static long? TryGetLong(JsonElement root, string propertyName) =>
        root.TryGetProperty(propertyName, out var node) && node.ValueKind == JsonValueKind.Number && node.TryGetInt64(out var value)
            ? value
            : null;

    private static string? TryGetFirstStringFromArray(JsonElement root, string propertyName)
    {
        if (!root.TryGetProperty(propertyName, out var node) || node.ValueKind != JsonValueKind.Array || node.GetArrayLength() == 0)
        {
            return null;
        }

        var first = node[0];
        return first.ValueKind == JsonValueKind.String ? first.GetString() : null;
    }

    private static IReadOnlyList<string> TryGetObjectValues(JsonElement root, string propertyName)
    {
        if (!root.TryGetProperty(propertyName, out var node) || node.ValueKind != JsonValueKind.Object)
        {
            return [];
        }

        return node.EnumerateObject()
            .Select(property => property.Value.ValueKind == JsonValueKind.String ? property.Value.GetString() : null)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(value => value!)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Order(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static IReadOnlyList<string> TryGetStringArray(JsonElement root, string propertyName)
    {
        if (!root.TryGetProperty(propertyName, out var node) || node.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        return node.EnumerateArray()
            .Select(item => item.ValueKind == JsonValueKind.String ? item.GetString() : null)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(value => value!)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Order(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static string? TryGetFirstCurrency(JsonElement root)
    {
        if (!root.TryGetProperty("currencies", out var currenciesNode) || currenciesNode.ValueKind != JsonValueKind.Object)
        {
            return null;
        }

        foreach (var currency in currenciesNode.EnumerateObject())
        {
            var code = currency.Name;
            var label = currency.Value.TryGetProperty("name", out var nameNode) && nameNode.ValueKind == JsonValueKind.String
                ? nameNode.GetString()
                : null;

            return string.IsNullOrWhiteSpace(label) ? code : $"{label} ({code})";
        }

        return null;
    }

    private static string? TryGetFlagUrl(JsonElement root)
    {
        if (!root.TryGetProperty("flags", out var flagsNode) || flagsNode.ValueKind != JsonValueKind.Object)
        {
            return null;
        }

        if (flagsNode.TryGetProperty("png", out var pngNode) && pngNode.ValueKind == JsonValueKind.String)
        {
            return pngNode.GetString();
        }

        return flagsNode.TryGetProperty("svg", out var svgNode) && svgNode.ValueKind == JsonValueKind.String
            ? svgNode.GetString()
            : null;
    }
}
