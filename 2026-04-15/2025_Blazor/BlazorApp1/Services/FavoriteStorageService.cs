using System.Text.Json;
using BlazorApp1.Models;
using Microsoft.JSInterop;

namespace BlazorApp1.Services;

public sealed class FavoriteStorageService(IJSRuntime jsRuntime)
{
    private const string StorageKey = "globebite.favorites.v1";
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public async Task<List<FavoriteItem>> LoadFavoritesAsync()
    {
        var rawJson = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);
        if (string.IsNullOrWhiteSpace(rawJson))
        {
            return [];
        }

        var favorites = JsonSerializer.Deserialize<List<FavoriteItem>>(rawJson, SerializerOptions);
        return favorites ?? [];
    }

    public async Task SaveFavoritesAsync(IEnumerable<FavoriteItem> favorites)
    {
        var rawJson = JsonSerializer.Serialize(favorites, SerializerOptions);
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, rawJson);
    }
}
