using System;

namespace Hausübung_2026_18_03;

public class Einkaufsliste
{
    private readonly string[] _artikel = new string[10];
    private int _anzahl;

    public int Anzahl => _anzahl;

    public bool VersucheHinzufuegen(string artikel, out string meldung)
    {
        if (string.IsNullOrWhiteSpace(artikel))
        {
            meldung = "Fehler: Artikelname darf nicht leer sein.";
            return false;
        }

        if (_anzahl >= _artikel.Length)
        {
            meldung = "Fehler: Die Einkaufsliste ist voll.";
            return false;
        }

        _artikel[_anzahl] = artikel;
        _anzahl++;

        meldung = $"Hinzugefuegt: {artikel}";
        return true;
    }

    public bool Enthält(string gesuchterArtikel)
    {
        bool gefunden = false;

        for (int i = 0; i < _anzahl; i++)
        {
            if (string.Equals(_artikel[i], gesuchterArtikel, StringComparison.OrdinalIgnoreCase))
            {
                gefunden = true;
                break;
            }
        }

        return gefunden;
    }

    public void GibKurzeNamenAus(int minLänge)
    {
        Console.WriteLine($"Artikel kuerzer als {minLänge} Zeichen:");

        for (int i = 0; i < _anzahl; i++)
        {
            string artikel = _artikel[i];

            if (artikel.Length >= minLänge)
            {
                continue;
            }

            Console.WriteLine($"- {artikel}");
        }
    }

    public static void VergleicheStrings(string erster, string zweiter)
    {
        bool ergebnisMitGleichGleich = erster == zweiter;
        bool ergebnisMitEquals = erster.Equals(zweiter);

        Console.WriteLine($"Mit ==: {ergebnisMitGleichGleich}");
        Console.WriteLine($"Mit .Equals(): {ergebnisMitEquals}");
        Console.WriteLine($"Gleiches Ergebnis: {ergebnisMitGleichGleich == ergebnisMitEquals}");
    }
}

public static class Program
{
    public static void Main()
    {
        Einkaufsliste liste = new();

        string[] testArtikel =
        [
            "Milch", "Brot", "Ei", "Butter", "Apfel", "Mehl",
            "Kakao", "Nudeln", "Reis", "Salz", "Zucker"
        ];

        foreach (string artikel in testArtikel)
        {
            bool erfolg = liste.VersucheHinzufuegen(artikel, out string meldung);
            Console.WriteLine($"{(erfolg ? "OK" : "NICHT OK")} - {meldung}");
        }

        Console.WriteLine();
        Console.WriteLine($"Aktuelle Anzahl: {liste.Anzahl}");
        Console.WriteLine($"Enthält 'Butter'? {liste.Enthält("Butter")}");
        Console.WriteLine($"Enthält 'Kaffee'? {liste.Enthält("Kaffee")}");
        Console.WriteLine();

        liste.GibKurzeNamenAus(6);
        Console.WriteLine();

        string a = "Milch";
        string b = "Mil" + "ch";
        Einkaufsliste.VergleicheStrings(a, b);
    }
}
