public class Bruch
{
    // jetzt kommen die sog. "Attribute" der Klasse oder "Felder"
    private int ganz;
    private int zaehler;
    private int nenner;

    public Bruch(string bruchtext)
    {
        string[] teile1 = bruchtext.Split(' ');
        this.ganz = int.Parse(teile1[0]);
        string[] teile = teile1[1].Split('/');
        this.zaehler = int.Parse(teile[0]);
        this.nenner = int.Parse(teile[1]);

        if (string.IsNullOrWhiteSpace(bruchtext))
            throw new FormatException("Bruch darf nicht leer sein.");
            
        if (this.nenner == 0)
            throw new FormatException("Nenner darf nicht 0 sein.");
    }

    public Bruch(int ganz, int zaehler, int nenner)
    {
        this.ganz = ganz;
        this.zaehler = zaehler;
        this.nenner = nenner;
    }

   private int GGT(int a, int b)
{
    while (b != 0)
    {
        int temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

public Bruch addiere(Bruch b)
{
    int bruch1Zaehler = this.zaehler + this.ganz * this.nenner;
    int bruch2Zaehler = b.zaehler + b.ganz * b.nenner;
    int gemeinsamerNenner = this.nenner * b.nenner;
    int neuerZaehler = (bruch1Zaehler * b.nenner) + (bruch2Zaehler * this.nenner);
    int ggt = GGT(neuerZaehler, gemeinsamerNenner);
    neuerZaehler /= ggt;
    gemeinsamerNenner /= ggt;
    int neueGanzzahl = neuerZaehler / gemeinsamerNenner;
    int restZaehler = neuerZaehler % gemeinsamerNenner;
    return new Bruch(neueGanzzahl, restZaehler, gemeinsamerNenner);
}



    public string toString()
    {
        if (this.zaehler == 0)
        {
            return $"ich bin ein bruch: {this.ganz}";
        }
        else
        {
            return $"ich bin ein bruch: {this.ganz} {this.zaehler}/{this.nenner}";
        }
    }
}