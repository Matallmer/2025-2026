using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World als Klasse!");

   if (args.Length != 2)
        {
            Console.WriteLine("Fehler: Bitte genau zwei Brüche im Format \"g z/n\" eingeben.");
            Console.WriteLine("Beispiel: dotnet run \"1 2/3\" \"0 3/4\"");
            return;
        }

        try
        {
            Bruch b1 = new Bruch(args[0]);
            Bruch b2 = new Bruch(args[1]);
            Bruch b3 = b1.addiere(b2);
            Console.WriteLine("Ergebnis: " + b3.toString());
        }
        catch (FormatException)
        {
            Console.WriteLine("Fehler: Ungültiges Format. Bitte im Format 'ganzzahl zaehler/nenner' eingeben.");
        }
    }
}
