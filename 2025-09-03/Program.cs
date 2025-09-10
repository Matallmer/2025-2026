using System;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Bitte 2 Brüche angeben, z.B.: dotnet run \"2 3/8\" \"1 5/6\"");
            return;
        }

        Fraction f1 = Fraction.Parse(args[0]);
        Fraction f2 = Fraction.Parse(args[1]);

        Fraction sum = f1 + f2;

        Console.WriteLine(sum.ToMixedString());
    }
}

public class Fraction
{
    public int Numerator { get; set; }
    public int Denominator { get; set; }

    public Fraction(int numerator, int denominator)
    {
        if (denominator == 0)
            throw new ArgumentException("Nenner darf nicht 0 sein.");
        Numerator = numerator;
        Denominator = denominator;
        Simplify();
    }

    public static Fraction Parse(string input)
    {
        // Erwartetes Format: "a b/c" oder nur "b/c" oder nur "a"
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 2) // gemischter Bruch
        {
            int whole = int.Parse(parts[0]);
            string[] frac = parts[1].Split('/');
            int num = int.Parse(frac[0]);
            int den = int.Parse(frac[1]);
            return new Fraction(whole * den + num, den);
        }
        else if (parts.Length == 1 && parts[0].Contains("/")) // echter Bruch
        {
            string[] frac = parts[0].Split('/');
            return new Fraction(int.Parse(frac[0]), int.Parse(frac[1]));
        }
        else if (parts.Length == 1) // ganze Zahl
        {
            return new Fraction(int.Parse(parts[0]), 1);
        }
        else
        {
            throw new FormatException("Ungültiges Eingabeformat.");
        }
    }

    public static Fraction operator +(Fraction a, Fraction b)
    {
        int numerator = a.Numerator * b.Denominator + b.Numerator * a.Denominator;
        int denominator = a.Denominator * b.Denominator;
        return new Fraction(numerator, denominator);
    }

    public void Simplify()
    {
        int g = GCD(Math.Abs(Numerator), Math.Abs(Denominator));
        Numerator /= g;
        Denominator /= g;
        if (Denominator < 0)
        {
            Numerator = -Numerator;
            Denominator = -Denominator;
        }
    }

    private int GCD(int a, int b)
    {
        while (b != 0)
        {
            int t = b;
            b = a % b;
            a = t;
        }
        return a;
    }

    public string ToMixedString()
    {
        int whole = Numerator / Denominator;
        int remainder = Math.Abs(Numerator % Denominator);

        if (remainder == 0) return whole.ToString();
        if (whole == 0) return $"{remainder}/{Denominator}";
        return $"{whole} {remainder}/{Denominator}";
    }
}

