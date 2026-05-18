using Microsoft.EntityFrameworkCore;

using var db = new WeatherTravelContext();
db.Database.EnsureCreated();
SeedData(db);

Console.WriteLine("Wetter-Reise EF Core Demo");
Console.WriteLine("Datenbank: wetterreise.db");

bool running = true;
while (running)
{
    PrintMenu();
    string choice = ReadText("Auswahl");

    switch (choice)
    {
        case "1":
            ListDestinations(db);
            break;
        case "2":
            CreateDestination(db);
            break;
        case "3":
            CreateForecast(db);
            break;
        case "4":
            UpdateDestination(db);
            break;
        case "5":
            UpdateForecast(db);
            break;
        case "6":
            DeleteDestination(db);
            break;
        case "7":
            DeleteForecast(db);
            break;
        case "0":
            running = false;
            break;
        default:
            Console.WriteLine("Ungueltige Auswahl.");
            break;
    }
}

static void PrintMenu()
{
    Console.WriteLine();
    Console.WriteLine("1 - Reiseziele mit Wetterdaten anzeigen");
    Console.WriteLine("2 - Reiseziel anlegen");
    Console.WriteLine("3 - Wetterdaten anlegen");
    Console.WriteLine("4 - Reiseziel bearbeiten");
    Console.WriteLine("5 - Wetterdaten bearbeiten");
    Console.WriteLine("6 - Reiseziel loeschen");
    Console.WriteLine("7 - Wetterdaten loeschen");
    Console.WriteLine("0 - Beenden");
}

static void ListDestinations(WeatherTravelContext db)
{
    List<Destination> destinations = db.Destinations
        .Include(destination => destination.Forecasts)
        .OrderBy(destination => destination.City)
        .ToList();

    if (destinations.Count == 0)
    {
        Console.WriteLine("Keine Reiseziele vorhanden.");
        return;
    }

    foreach (Destination destination in destinations)
    {
        Console.WriteLine();
        Console.WriteLine($"{destination.Id}: {destination.City}, {destination.Country}");
        Console.WriteLine($"   Bester Reisemonat: {destination.BestTravelMonth}");
        Console.WriteLine($"   Notiz: {destination.Notes}");

        if (destination.Forecasts.Count == 0)
        {
            Console.WriteLine("   Keine Wetterdaten vorhanden.");
            continue;
        }

        foreach (WeatherForecast forecast in destination.Forecasts.OrderBy(forecast => forecast.Date))
        {
            Console.WriteLine(
                $"   Wetter #{forecast.Id}: {forecast.Date:dd.MM.yyyy}, {forecast.TemperatureCelsius:F1} Grad, {forecast.Condition}, Regen {forecast.RainChancePercent}%");
        }
    }
}

static void CreateDestination(WeatherTravelContext db)
{
    Destination destination = new()
    {
        City = ReadText("Stadt"),
        Country = ReadText("Land"),
        BestTravelMonth = ReadText("Bester Reisemonat"),
        Notes = ReadText("Notiz")
    };

    db.Destinations.Add(destination);
    db.SaveChanges();

    Console.WriteLine($"Reiseziel #{destination.Id} wurde angelegt.");
}

static void CreateForecast(WeatherTravelContext db)
{
    Destination? destination = FindDestination(db);
    if (destination is null)
    {
        return;
    }

    WeatherForecast forecast = new()
    {
        DestinationId = destination.Id,
        Date = ReadDate("Datum"),
        TemperatureCelsius = ReadDouble("Temperatur in Grad Celsius"),
        Condition = ReadText("Wetterlage"),
        RainChancePercent = ReadInt("Regenwahrscheinlichkeit in Prozent")
    };

    db.WeatherForecasts.Add(forecast);
    db.SaveChanges();

    Console.WriteLine($"Wetterdaten #{forecast.Id} wurden angelegt.");
}

static void UpdateDestination(WeatherTravelContext db)
{
    Destination? destination = FindDestination(db);
    if (destination is null)
    {
        return;
    }

    destination.City = ReadText($"Stadt ({destination.City})");
    destination.Country = ReadText($"Land ({destination.Country})");
    destination.BestTravelMonth = ReadText($"Bester Reisemonat ({destination.BestTravelMonth})");
    destination.Notes = ReadText($"Notiz ({destination.Notes})");

    db.SaveChanges();
    Console.WriteLine("Reiseziel wurde aktualisiert.");
}

static void UpdateForecast(WeatherTravelContext db)
{
    WeatherForecast? forecast = FindForecast(db);
    if (forecast is null)
    {
        return;
    }

    forecast.Date = ReadDate($"Datum ({forecast.Date:dd.MM.yyyy})");
    forecast.TemperatureCelsius = ReadDouble($"Temperatur ({forecast.TemperatureCelsius:F1})");
    forecast.Condition = ReadText($"Wetterlage ({forecast.Condition})");
    forecast.RainChancePercent = ReadInt($"Regenwahrscheinlichkeit ({forecast.RainChancePercent})");

    db.SaveChanges();
    Console.WriteLine("Wetterdaten wurden aktualisiert.");
}

static void DeleteDestination(WeatherTravelContext db)
{
    Destination? destination = FindDestination(db);
    if (destination is null)
    {
        return;
    }

    db.Destinations.Remove(destination);
    db.SaveChanges();

    Console.WriteLine("Reiseziel wurde geloescht. Zugehoerige Wetterdaten wurden ebenfalls entfernt.");
}

static void DeleteForecast(WeatherTravelContext db)
{
    WeatherForecast? forecast = FindForecast(db);
    if (forecast is null)
    {
        return;
    }

    db.WeatherForecasts.Remove(forecast);
    db.SaveChanges();

    Console.WriteLine("Wetterdaten wurden geloescht.");
}

static Destination? FindDestination(WeatherTravelContext db)
{
    int id = ReadInt("Reiseziel-Id");
    Destination? destination = db.Destinations.Find(id);

    if (destination is null)
    {
        Console.WriteLine("Reiseziel wurde nicht gefunden.");
    }

    return destination;
}

static WeatherForecast? FindForecast(WeatherTravelContext db)
{
    int id = ReadInt("Wetterdaten-Id");
    WeatherForecast? forecast = db.WeatherForecasts.Find(id);

    if (forecast is null)
    {
        Console.WriteLine("Wetterdaten wurden nicht gefunden.");
    }

    return forecast;
}

static string ReadText(string label)
{
    while (true)
    {
        Console.Write($"{label}: ");
        string? input = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(input))
        {
            return input.Trim();
        }

        Console.WriteLine("Bitte einen Wert eingeben.");
    }
}

static int ReadInt(string label)
{
    while (true)
    {
        string input = ReadText(label);

        if (int.TryParse(input, out int number))
        {
            return number;
        }

        Console.WriteLine("Bitte eine ganze Zahl eingeben.");
    }
}

static double ReadDouble(string label)
{
    while (true)
    {
        string input = ReadText(label);

        if (double.TryParse(input, out double number))
        {
            return number;
        }

        Console.WriteLine("Bitte eine Zahl eingeben.");
    }
}

static DateOnly ReadDate(string label)
{
    while (true)
    {
        string input = ReadText($"{label} (yyyy-mm-dd)");

        if (DateOnly.TryParse(input, out DateOnly date))
        {
            return date;
        }

        Console.WriteLine("Bitte ein gueltiges Datum eingeben, z.B. 2026-07-15.");
    }
}

static void SeedData(WeatherTravelContext db)
{
    if (db.Destinations.Any())
    {
        return;
    }

    Destination split = new()
    {
        City = "Split",
        Country = "Kroatien",
        BestTravelMonth = "Juni",
        Notes = "Warm, aber noch nicht zu ueberlaufen",
        Forecasts =
        [
            new WeatherForecast
            {
                Date = new DateOnly(2026, 6, 15),
                TemperatureCelsius = 27.5,
                Condition = "sonnig",
                RainChancePercent = 10
            }
        ]
    };

    Destination bergen = new()
    {
        City = "Bergen",
        Country = "Norwegen",
        BestTravelMonth = "August",
        Notes = "Gute Wahl fuer mildes Wetter und Natur",
        Forecasts =
        [
            new WeatherForecast
            {
                Date = new DateOnly(2026, 8, 5),
                TemperatureCelsius = 18.0,
                Condition = "wechselhaft",
                RainChancePercent = 55
            }
        ]
    };

    db.Destinations.AddRange(split, bergen);
    db.SaveChanges();
}

public class WeatherTravelContext : DbContext
{
    public DbSet<Destination> Destinations => Set<Destination>();

    public DbSet<WeatherForecast> WeatherForecasts => Set<WeatherForecast>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=wetterreise.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Destination>(entity =>
        {
            entity.ToTable("Destinations");
            entity.HasKey(destination => destination.Id);
            entity.Property(destination => destination.City).HasMaxLength(100).IsRequired();
            entity.Property(destination => destination.Country).HasMaxLength(100).IsRequired();
            entity.Property(destination => destination.BestTravelMonth).HasMaxLength(30).IsRequired();
            entity.Property(destination => destination.Notes).HasMaxLength(300).IsRequired();
        });

        modelBuilder.Entity<WeatherForecast>(entity =>
        {
            entity.ToTable("WeatherForecasts");
            entity.HasKey(forecast => forecast.Id);
            entity.Property(forecast => forecast.Condition).HasMaxLength(80).IsRequired();
            entity.HasOne(forecast => forecast.Destination)
                .WithMany(destination => destination.Forecasts)
                .HasForeignKey(forecast => forecast.DestinationId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

public class Destination
{
    public int Id { get; set; }

    public string City { get; set; } = "";

    public string Country { get; set; } = "";

    public string BestTravelMonth { get; set; } = "";

    public string Notes { get; set; } = "";

    public List<WeatherForecast> Forecasts { get; set; } = [];
}

public class WeatherForecast
{
    public int Id { get; set; }

    public int DestinationId { get; set; }

    public Destination? Destination { get; set; }

    public DateOnly Date { get; set; }

    public double TemperatureCelsius { get; set; }

    public string Condition { get; set; } = "";

    public int RainChancePercent { get; set; }
}
