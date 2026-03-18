using System;

namespace Bruch
{
    public class Bruch
    {
        private int ganz;
        private int zaehler;
        private int nenner;

        public Bruch(string bruchtext)
        {
            if (string.IsNullOrWhiteSpace(bruchtext))
                throw new ArgumentException("Bruchtext darf nicht leer sein.");

            string[] teile1 = bruchtext.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (teile1.Length != 2)
                throw new FormatException($"Format muss 'Ganzzahl Zähler/Nenner' sein, z.B. '1 1/2', erhalten: '{bruchtext}'");

            if (!int.TryParse(teile1[0], out this.ganz))
                throw new FormatException($"Ganzzahl '{teile1[0]}' ist keine gültige Ganzzahl");

            string[] teile = teile1[1].Split('/');
            if (teile.Length != 2)
                throw new FormatException($"Bruch muss im Format 'Zähler/Nenner' sein, z.B. '1/2', erhalten: '{teile1[1]}'");

            if (!int.TryParse(teile[0], out this.zaehler))
                throw new FormatException($"Zähler '{teile[0]}' ist keine gültige Ganzzahl");

            if (!int.TryParse(teile[1], out this.nenner))
                throw new FormatException($"Nenner '{teile[1]}' ist keine gültige Ganzzahl");

            if (this.nenner == 0)
                throw new ArgumentException("Nenner darf nicht 0 sein");

            this.KuerzeBruch();
        }

        public Bruch(int ganz, int zaehler, int nenner)
        {
            if (nenner == 0)
                throw new ArgumentException("Nenner darf nicht 0 sein");

            this.ganz = ganz;
            this.zaehler = zaehler;
            this.nenner = nenner;

            this.KuerzeBruch();
        }

        private void KuerzeBruch()
        {
            if (this.zaehler == 0)
            {
                this.nenner = 1;
                return;
            }

            int ggt = GGT(Math.Abs(this.zaehler), Math.Abs(this.nenner));

            this.zaehler /= ggt;
            this.nenner /= ggt;

            if (this.zaehler >= this.nenner)
            {
                this.ganz += this.zaehler / this.nenner;
                this.zaehler = this.zaehler % this.nenner;
            }
        }

        private static int GGT(int a, int b)
        {
            if (a == 0) return b;
            if (b == 0) return a;
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return Math.Abs(a);
        }

        public Bruch Addiere(Bruch b)
        {
            int gemeinsamerNenner = this.nenner * b.nenner;
            int neuerZaehler = (this.zaehler * b.nenner) + (b.zaehler * this.nenner);
            int neuerGanz = this.ganz + b.ganz + neuerZaehler / gemeinsamerNenner;
            int neuerBruchZaehler = Math.Abs(neuerZaehler % gemeinsamerNenner);

            int ggt = GGT(neuerBruchZaehler, Math.Abs(gemeinsamerNenner));
            if (ggt == 0)
            {
                return new Bruch(neuerGanz, 0, 1);
            }

            neuerBruchZaehler /= ggt;
            gemeinsamerNenner = Math.Abs(gemeinsamerNenner) / ggt;

            return new Bruch(neuerGanz, neuerBruchZaehler, gemeinsamerNenner);
        }

        public override string ToString()
        {
            if (this.zaehler == 0)
            {
                return $"{this.ganz}";
            }
            else
            {
                return $"{this.ganz} {this.zaehler}/{this.nenner}";
            }
        }
    }
}
