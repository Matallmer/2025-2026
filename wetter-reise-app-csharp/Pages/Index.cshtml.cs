using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WetterReiseApp.Models;
using WetterReiseApp.Services;

namespace WetterReiseApp.Pages;

public class IndexModel : PageModel
{
    private readonly WeatherTravelService _weatherTravelService;

    public IndexModel(WeatherTravelService weatherTravelService)
    {
        _weatherTravelService = weatherTravelService;
    }

    [BindProperty(SupportsGet = true)]
    public string? City { get; set; }

    [BindProperty(SupportsGet = true)]
    public double? Lat { get; set; }

    [BindProperty(SupportsGet = true)]
    public double? Lon { get; set; }

    public WeatherTravelViewModel? Weather { get; private set; }

    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        try
        {
            Weather = Lat.HasValue && Lon.HasValue
                ? await _weatherTravelService.GetForCoordinatesAsync(Lat.Value, Lon.Value, cancellationToken)
                : await _weatherTravelService.GetForCityAsync(City, cancellationToken);
        }
        catch (Exception exception)
        {
            ErrorMessage = exception.Message;
        }
    }
}
