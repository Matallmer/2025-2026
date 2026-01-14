using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

var csvPath = Path.Combine(Directory.GetCurrentDirectory(), "persons.csv");
if (!File.Exists(csvPath))
{
    Console.Error.WriteLine($"Error: {csvPath} not found. Run from project folder so that persons.csv is next to the .csproj.");
    return;
}

var config = new CsvConfiguration(CultureInfo.InvariantCulture)
{
    Delimiter = ";",
    MissingFieldFound = null,
    HeaderValidated = null,
    BadDataFound = null,
};

try
{
    using var reader = new StreamReader(csvPath);
    using var csv = new CsvReader(reader, config);

    var records = csv.GetRecords<Person>();
    Console.WriteLine("Loaded persons:\n");
    foreach (var p in records)
    {
        Console.WriteLine($"Name: {p.Fullname}");
        Console.WriteLine($"  Email: {p.Email}");
        Console.WriteLine($"  Telefon: {p.Telefon}");
        Console.WriteLine($"  Adresse: {p.Adresse}");
        Console.WriteLine();
    }
}
catch (Exception ex)
{
    Console.Error.WriteLine($"An error occurred: {ex.Message}");
}

public class Person
{
    public string Fullname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefon { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public string unicode { get; set; } = string.Empty;
}