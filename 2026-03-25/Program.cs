using System;
using System.Collections.Generic;
using System.Linq;

namespace Lagerverwaltung_2026_03_25;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Schritt 1: Array (Das feste Regal) ===");

        string[] regalPlaetze =
        [
            "Kupplung",
            "Bremsbelag",
            "Filter",
            "Lichtmaschine",
            "Zahnriemen"
        ];

        regalPlaetze = regalPlaetze.OrderBy(teil => teil).ToArray();

        foreach (string teil in regalPlaetze)
        {
            Console.WriteLine(teil);
        }

        Console.WriteLine();
        Console.WriteLine("=== Schritt 2: List (Die dynamische Einlagerung) ===");

        List<string> eingangskorb = new();
        eingangskorb.Add("Schraube");
        eingangskorb.Add("Mutter");
        eingangskorb.Add("Bolzen");
        eingangskorb.Add("Feder");

        eingangskorb.RemoveAt(1);

        if (eingangskorb.Contains("Schraube"))
        {
            Console.WriteLine("Schraube ist noch im Eingangskorb enthalten.");
        }
        else
        {
            Console.WriteLine("Schraube ist nicht mehr im Eingangskorb enthalten.");
        }

        Console.WriteLine($"Verbleibende Teile im Eingangskorb: {eingangskorb.Count}");

        Console.WriteLine();
        Console.WriteLine("=== Schritt 3: Dictionary (Das Such-System) ===");

        Dictionary<int, string> artikelVerzeichnis = new()
        {
            { 101, "Motor" },
            { 102, "Getriebe" },
            { 103, "Reifen" }
        };

        FindArtikel(artikelVerzeichnis, 102);
        FindArtikel(artikelVerzeichnis, 999);

        Console.WriteLine();
        Console.WriteLine("Alle Eintraege im Artikelverzeichnis:");
        foreach (KeyValuePair<int, string> eintrag in artikelVerzeichnis)
        {
            Console.WriteLine($"ID: {eintrag.Key}, Teil: {eintrag.Value}");
        }

        Console.WriteLine();
        Console.WriteLine("=== Schritt 4: Abschluss ===");
        Console.WriteLine("Alle Schritte wurden erfolgreich nacheinander ausgefuehrt.");
    }

    private static void FindArtikel(Dictionary<int, string> artikelVerzeichnis, int id)
    {
        if (artikelVerzeichnis.TryGetValue(id, out string? artikelName))
        {
            Console.WriteLine($"Gefunden: ID {id} gehoert zu '{artikelName}'.");
        }
        else
        {
            Console.WriteLine($"Fehler: Die ID {id} ist unbekannt.");
        }
    }
}
