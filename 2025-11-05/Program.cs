using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World als Klasse!");

        if (args.Length < 2)
        {
            Console.WriteLine("Fehler: Bitte zwei Brüche im Format 'ganzzahl zaehler/nenner' eingeben.");
            return;
        }

        foreach (string s in args)
        {
            Console.WriteLine(s);
        }

        try
        {
            Bruch b1 = new Bruch(args[0]);
            Bruch b2 = new Bruch(args[1]);
            Bruch b3 = b1.addiere(b2);
            Console.WriteLine("Ergebnis: " + b3.toString());
        }
        catch (FormatException ex)
        {
            Console.WriteLine("Fehler: Ungültiges Format. Bitte im Format 'ganzzahl zaehler/nenner' eingeben.");
        }
    }
}
