using System;
using System.Collections.Generic;

namespace Inventarsystem;

public interface IInventarGegenstand
{
    string Name { get; }
    string BeschreibeDich();
}

public class Waffe : IInventarGegenstand
{
    public string Name { get; }
    public int Schaden { get; }

    public Waffe(string name, int schaden)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name darf nicht leer sein.", nameof(name));
        }

        if (schaden <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(schaden), "Schaden muss groesser als 0 sein.");
        }

        Name = name;
        Schaden = schaden;
    }

    public string BeschreibeDich()
    {
        return $"Ich bin die Waffe {Name} und mache {Schaden} Schaden.";
    }
}

public class Heiltrank : IInventarGegenstand
{
    public string Name { get; }
    public int Heilwert { get; }

    public Heiltrank(string name, int heilwert)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name darf nicht leer sein.", nameof(name));
        }

        if (heilwert <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(heilwert), "Heilwert muss groesser als 0 sein.");
        }

        Name = name;
        Heilwert = heilwert;
    }

    public string BeschreibeDich()
    {
        return $"Ich bin der Heiltrank {Name} und stelle {Heilwert} Lebenspunkte wieder her.";
    }
}

public class Program
{
    public static void Main()
    {
        List<IInventarGegenstand> inventar =
        [
            new Waffe("Schwert", 15),
            new Heiltrank("Kleiner Trank", 25)
        ];

        foreach (IInventarGegenstand gegenstand in inventar)
        {
            Console.WriteLine(gegenstand.BeschreibeDich());
        }
    }
}
