using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using BlazorApp1.Models;

namespace BlazorApp1.Services;

public sealed class OpenFoodFactsService(IHttpClientFactory httpClientFactory)
{
    public async Task<IReadOnlyList<FoodProductSummary>> SearchProductsAsync(
        string query,
        int pageSize = 12,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return [];
        }

        var client = httpClientFactory.CreateClient();
        var url =
            $"https://world.openfoodfacts.org/cgi/search.pl?search_terms={Uri.EscapeDataString(query)}&search_simple=1&action=process&json=1&page_size={pageSize}&fields=code,product_name,brands,image_front_small_url,nutriscore_grade";

        var payload = await client.GetFromJsonAsync<SearchResponse>(url, cancellationToken);
        if (payload?.Products is null)
        {
            return [];
        }

        return payload.Products
            .Where(product => product is not null && !string.IsNullOrWhiteSpace(product.Code))
            .Select(product => new FoodProductSummary
            {
                Code = product!.Code!,
                Name = string.IsNullOrWhiteSpace(product.ProductName) ? $"Produkt {product.Code}" : product.ProductName,
                Brand = product.Brands,
                ImageUrl = product.ImageFrontSmallUrl,
                NutriScore = NormalizeNutriScore(product.NutriScore)
            })
            .ToList();
    }

    public async Task<FoodProductDetails?> GetProductDetailsAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return null;
        }

        var client = httpClientFactory.CreateClient();
        var safeCode = Uri.EscapeDataString(code.Trim());
        var url =
            $"https://world.openfoodfacts.org/api/v2/product/{safeCode}.json?fields=code,product_name,brands,countries,ingredients_text,image_front_url,nutriscore_grade,nutriments";

        using var response = await client.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var json = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
        var root = json.RootElement;

        if (root.TryGetProperty("status", out var statusNode) &&
            statusNode.ValueKind == JsonValueKind.Number &&
            statusNode.TryGetInt32(out var statusCode) &&
            statusCode == 0)
        {
            return null;
        }

        if (!root.TryGetProperty("product", out var productNode) || productNode.ValueKind != JsonValueKind.Object)
        {
            return null;
        }

        var resolvedCode = GetString(productNode, "code") ?? code.Trim();
        var name = GetString(productNode, "product_name");
        var nutrimentsNode = productNode.TryGetProperty("nutriments", out var rawNutriments) ? rawNutriments : default;

        return new FoodProductDetails
        {
            Code = resolvedCode,
            Name = string.IsNullOrWhiteSpace(name) ? $"Produkt {resolvedCode}" : name,
            Brand = GetString(productNode, "brands"),
            ImageUrl = GetString(productNode, "image_front_url"),
            Countries = GetString(productNode, "countries"),
            Ingredients = GetString(productNode, "ingredients_text"),
            NutriScore = NormalizeNutriScore(GetString(productNode, "nutriscore_grade")),
            EnergyKcal100g = GetDouble(nutrimentsNode, "energy-kcal_100g"),
            Fat100g = GetDouble(nutrimentsNode, "fat_100g"),
            Carbohydrates100g = GetDouble(nutrimentsNode, "carbohydrates_100g"),
            Sugars100g = GetDouble(nutrimentsNode, "sugars_100g"),
            Protein100g = GetDouble(nutrimentsNode, "proteins_100g"),
            Salt100g = GetDouble(nutrimentsNode, "salt_100g")
        };
    }

    private static string? GetString(JsonElement node, string propertyName) =>
        node.ValueKind == JsonValueKind.Object &&
        node.TryGetProperty(propertyName, out var propertyNode) &&
        propertyNode.ValueKind == JsonValueKind.String
            ? propertyNode.GetString()
            : null;

    private static double? GetDouble(JsonElement node, string propertyName)
    {
        if (node.ValueKind != JsonValueKind.Object ||
            !node.TryGetProperty(propertyName, out var propertyNode))
        {
            return null;
        }

        if (propertyNode.ValueKind == JsonValueKind.Number && propertyNode.TryGetDouble(out var numericValue))
        {
            return numericValue;
        }

        if (propertyNode.ValueKind == JsonValueKind.String &&
            double.TryParse(propertyNode.GetString(), CultureInfo.InvariantCulture, out var textValue))
        {
            return textValue;
        }

        return null;
    }

    private static string? NormalizeNutriScore(string? rawValue) =>
        string.IsNullOrWhiteSpace(rawValue) ? null : rawValue.Trim().ToUpperInvariant();

    private sealed class SearchResponse
    {
        [JsonPropertyName("products")]
        public List<SearchProduct?>? Products { get; set; }
    }

    private sealed class SearchProduct
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("product_name")]
        public string? ProductName { get; set; }

        [JsonPropertyName("brands")]
        public string? Brands { get; set; }

        [JsonPropertyName("image_front_small_url")]
        public string? ImageFrontSmallUrl { get; set; }

        [JsonPropertyName("nutriscore_grade")]
        public string? NutriScore { get; set; }
    }
}
