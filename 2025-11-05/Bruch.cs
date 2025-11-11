class Bruch
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
    }

    public Bruch(int ganz, int zaehler, int nenner)
    {
        this.ganz = ganz;
        this.zaehler = zaehler;
        this.nenner = nenner;
    }

    public Bruch addiere(Bruch b)
    {
        int gemeinsamerNenner = this.nenner * b.nenner;
        int neuerZaehler = (this.zaehler * b.nenner) + (b.zaehler * this.nenner);
        int neuerGanz = this.ganz + b.ganz + neuerZaehler / gemeinsamerNenner;
        int neuerBruchZaehler = neuerZaehler % gemeinsamerNenner;

        return new Bruch(neuerGanz, neuerBruchZaehler, gemeinsamerNenner);
    }

    public string toString()
    {
        return $"ich bin ein bruch: {this.zaehler}/{this.nenner}";
        // JS: return `ich bin ein bruch: ${this.zaehler}/${this.nenner}`;
    }
}