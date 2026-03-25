# Knowledge Check – 3AHWII

**Datum:** 24.03.2026  
**Themen:** Interfaces, ref/out, Strings, Schleifen (break/continue), ENUM, Fail Fast

**Punkte:** 85 (10 MC à 4 Punkte + 3 Freitext à 15 Punkte)

---

## Teil 1: Multiple-Choice (40 Punkte)

_Jede Frage hat 4 Antwortoptionen. Kreuze alle zutreffenden Optionen an.  
Pro Frage gibt es 4 Punkte: Für jede korrekt angekreuzte Option 1 Punkt, für jede korrekt nicht angekreuzte Option 1 Punkt._

_Falls du eine Frage als mehrdeutig empfindest, kannst du sie mit `-` markieren und kurz begründen._

---

### Frage 1: Interfaces – Grundlagen (4 Punkte)

Welche Aussagen über Interfaces in C# sind korrekt?

- [ ] A) Ein Interface kann Instanzfelder (z.B. `private int zahl;`) enthalten.
- [x] B) Ein Interface definiert nur Signaturen von Methoden, Properties oder Events, aber keine Implementierung.
- [x] C) Eine Klasse kann mehrere Interfaces gleichzeitig implementieren.
- [x] D) Eine Klasse, die ein Interface implementiert, muss alle Member des Interfaces bereitstellen.

---

### Frage 2: Interfaces – Polymorphie (4 Punkte)

Gegeben sei:

```csharp
interface IInventarGegenstand
{
    string Name { get; }
    string BeschreibeDich();
}

class Waffe : IInventarGegenstand { ... }
class Heiltrank : IInventarGegenstand { ... }
```

Welche Aussagen sind korrekt?

- [x] A) `List<IInventarGegenstand>` kann sowohl `Waffe`- als auch `Heiltrank`-Objekte aufnehmen.
- [ ] B) Man kann auf ein `Waffe`-Objekt in der Liste nur zugreifen, wenn man es explizit zu `Waffe` castet.
- [x] C) `foreach (var item in liste) { item.BeschreibeDich(); }` funktioniert für alle Objekte in der Liste.
- [ ] D) Eine Klasse kann nicht gleichzeitig `IInventarGegenstand` und ein anderes Interface implementieren.

---

### Frage 3: Interfaces vs. Klassen (4 Punkte)

Worin unterscheiden sich Interfaces von abstrakten Klassen in C#?

- [x] A) Eine abstrakte Klasse kann implementierte Methoden enthalten, ein Interface (vor C# 8) nicht.
- [x] B) Eine Klasse kann von mehreren Interfaces erben, aber nur von einer Basisklasse.
- [ ] C) Interfaces können Konstruktoren definieren.
- [ ] D) Abstrakte Klassen können nicht instanziiert werden, Interfaces hingegen schon.

---

### Frage 4: `out`-Parameter (4 Punkte)

Welche Aussagen über `out`-Parameter sind korrekt?

- [x] A) Eine Methode mit `out`-Parameter muss diesem Parameter zwingend einen Wert zuweisen, bevor sie zurückkehrt.
- [ ] B) Der an `out` übergebene Variable muss vor dem Aufruf initialisiert sein.
- [x] C) `out` wird verwendet, um mehrere Werte aus einer Methode zurückzugeben.
- [ ] D) `out`-Parameter werden als Wert (by value) übergeben.

---

### Frage 5: `ref` vs `out` (4 Punkte)

Was ist der Unterschied zwischen `ref` und `out`?

- [x] A) Bei `ref` muss die Variable vor dem Aufruf initialisiert sein, bei `out` nicht.
- [x] B) Bei `out` muss die Methode der Variable zwingend einen Wert zuweisen, bei `ref` nicht.
- [ ] C) `ref` und `out` sind völlig identisch und austauschbar.
- [x] D) Beide Schlüsselwörter bewirken, dass Änderungen an der Variable im Aufrufer sichtbar sind.

---

### Frage 6: String-Vergleich `==` vs `.Equals()` (4 Punkte)

```csharp
string a = "Hallo";
string b = "Hal" + "lo";
```

Welche Aussagen sind korrekt?

- [x] A) `a == b` gibt `true` zurück.
- [x] B) `a.Equals(b)` gibt `true` zurück.
- [ ] C) `==` vergleicht die Referenz (Speicheradresse), `.Equals()` vergleicht den Inhalt.
- [x] D) Bei Strings verhalten sich `==` und `.Equals()` in C# identisch, wenn beide Strings zur Laufzeit denselben Inhalt haben.

---

### Frage 7: StringBuilder (4 Punkte)

Wann ist die Verwendung von `StringBuilder` statt String-Konkatenation sinnvoll?

- [ ] A) Bei einmaliger Verkettung von zwei Strings.
- [x] B) Bei vielen String-Operationen in einer Schleife.
- [x] C) Wenn Strings in einem großen Puffer effizient zusammengebaut werden sollen.
- [ ] D) `StringBuilder` sollte immer verwendet werden, da er schneller ist als `+`.

---

### Frage 8: `break` in Schleifen (4 Punkte)

```csharp
for (int i = 0; i < 10; i++)
{
    if (i == 5) break;
    Console.WriteLine(i);
}
```

Welche Aussagen sind korrekt?

- [x] A) Die Ausgabe ist: `0 1 2 3 4`
- [ ] B) Die Ausgabe ist: `0 1 2 3 4 5`
- [x] C) `break` beendet die Schleife sofort und setzt mit dem Code nach der Schleife fort.
- [ ] D) Nach `break` wird die Schleife noch einmal vollständig durchlaufen.

---

### Frage 9: `continue` in Schleifen (4 Punkte)

```csharp
for (int i = 0; i < 5; i++)
{
    if (i == 2) continue;
    Console.WriteLine(i);
}
```

Welche Aussagen sind korrekt?

- [x] A) Die Ausgabe ist: `0 1 3 4`
- [ ] B) Die Ausgabe ist: `0 1 2 3 4`
- [x] C) `continue` überspringt den Rest des aktuellen Schleifendurchlaufs und fährt mit dem nächsten Durchlauf fort.
- [ ] D) `continue` beendet die gesamte Schleife.

---

### Frage 10: ENUM und Fail Fast (4 Punkte)

Welche Aussagen sind korrekt?

- [x] A) Ein `enum` in C# ist ein Wertetyp, der eine Gruppe benannter Konstanten definiert.
- [x] B) "Fail Fast" bedeutet, Fehler so früh wie möglich zu erkennen und zu behandeln (z.B. durch frühe `throw`-Anweisungen).
- [ ] C) Bei "Fail Fast" werden Fehler am Ende der Methode gesammelt und dann geworfen.
- [x] D) Enums können nur numerische Werte (`int` standardmäßig) annehmen.

---

## Teil 2: Freitext-Fragen (45 Punkte)

### Aufgabe 1: Interface entwerfen und implementieren (15 Punkte)

**Ausgangslage:** Du sollst ein System für verschiedene Fahrzeuge entwerfen.

**Aufgaben:**

a) **(3 Punkte)** Definiere ein Interface `IFahrzeug` mit:

- Property `string Typ { get; }`
- Methode `string StarteMotor()`

b) **(6 Punkte)** Implementiere zwei Klassen `Auto` und `Fahrrad`, die `IFahrzeug` implementieren:

- `Auto`: `Typ` gibt "Auto" zurück, `StarteMotor()` gibt "Motor läuft"
- `Fahrrad`: `Typ` gibt "Fahrrad" zurück, `StarteMotor()` gibt "Fahrräder haben keinen Motor"

c) **(3 Punkte)** Schreibe Code, der eine `List<IFahrzeug>` erstellt, ein Auto und ein Fahrrad hinzufügt, und dann in einer Schleife für jedes Fahrzeug `Typ` und `StarteMotor()` ausgibt.

d) **(3 Punkte)** Erkläre kurz (2-3 Sätze), warum es sinnvoll ist, beide Klassen in derselben Liste zu speichern.

## Lösung

```csharp
using System;
using System.Collections.Generic;

interface IFahrzeug
{
    string Typ { get; }
    string StarteMotor();
}

class Auto : IFahrzeug
{
    public string Typ => "Auto";

    public string StarteMotor()
    {
        return "Motor läuft";
    }
}

class Fahrrad : IFahrzeug
{
    public string Typ => "Fahrrad";

    public string StarteMotor()
    {
        return "Fahrräder haben keinen Motor";
    }
}

class Program
{
    static void Main()
    {
        List<IFahrzeug> fahrzeuge = new List<IFahrzeug>
        {
            new Auto(),
            new Fahrrad()
        };

        foreach (IFahrzeug fahrzeug in fahrzeuge)
        {
            Console.WriteLine($"{fahrzeug.Typ}: {fahrzeug.StarteMotor()}");
        }
    }
}
```

**d)**  
Das Interface `IFahrzeug` stellt sicher, dass alle Fahrzeugtypen dieselben grundlegenden Member anbieten. Dadurch kann man unterschiedliche Klassen (`Auto`, `Fahrrad`) einheitlich behandeln, z.B. in einer gemeinsamen Liste. Das macht den Code flexibler und leichter erweiterbar (z.B. später `Motorrad`).

---

### Aufgabe 2: `ref` und `out` – Fehler finden und korrigieren (15 Punkte)

**Ausgangslage:** Der folgende Code enthält mehrere Fehler im Umgang mit `ref` und `out`.

```csharp
class Program
{
    static void Main()
    {
        int x;           // Zeile 5
        int y = 10;      // Zeile 6

        Berechne(x, out y);  // Zeile 8

        Verdopple(ref x);     // Zeile 10
    }

    static void Berechne(out int a, out int b)
    {
        b = a * 2;  // Zeile 15
    }

    static void Verdopple(ref int zahl)
    {
        // nichts hier
    }
}
```

**Aufgaben:**

a) **(6 Punkte)** Finde alle Fehler im Code und beschreibe für jeden Fehler:

- Zeilennummer
- Was ist falsch?
- Warum ist es falsch?

b) **(6 Punkte)** Schreibe den korrigierten Code.

c) **(3 Punkte)** Erkläre den Unterschied zwischen `ref` und `out` in 2-3 Sätzen.

## Lösung

**a)**

- Zeile 8: `Berechne(x, out y);`  
  Fehler: Für den ersten Parameter fehlt `out`.  
  Warum: Die Methode erwartet `out int a`, daher muss auch beim Aufruf `out` angegeben werden.

- Zeile 15: `b = a * 2;`  
  Fehler: `a` wird gelesen, bevor es in der Methode gesetzt wurde.  
  Warum: `out`-Parameter müssen vor jeder Verwendung zuerst zugewiesen werden.

- Methode `Berechne`: `a` wird nie zugewiesen  
  Fehler: `a` bekommt keinen Wert, obwohl es ein `out`-Parameter ist.  
  Warum: Jeder `out`-Parameter muss vor dem Verlassen der Methode zwingend gesetzt sein.

**b)**

```csharp
class Program
{
    static void Main()
    {
        int x;
        int y = 10;

        Berechne(out x, out y);
        Verdopple(ref x);
    }

    static void Berechne(out int a, out int b)
    {
        a = 5;
        b = a * 2;
    }

    static void Verdopple(ref int zahl)
    {
        zahl *= 2;
    }
}
```

**c)**  
Bei `ref` muss die Variable vor dem Methodenaufruf bereits initialisiert sein. Bei `out` ist das nicht nötig, dafür muss die Methode der Variable unbedingt einen Wert zuweisen. Beide Übergabearten arbeiten per Referenz, daher sind Änderungen nach dem Aufruf im Aufrufer sichtbar.

---

### Aufgabe 3: `break` und `continue` anwenden (15 Punkte)

**Ausgangslage:** Du hast ein Array mit Artikelnamen:

```csharp
string[] artikel = { "Apfel", "Banane", "Kiwi", "Orange", "Mango" };
```

**Aufgaben:**

a) **(5 Punkte)** Schreibe eine Schleife, die alle Artikel ausgibt, aber die Schleife mit `break` beendet, sobald "Kiwi" gefunden wird.  
 _Erwartete Ausgabe:_ `Apfel Banane`

b) **(5 Punkte)** Schreibe eine Schleife, die alle Artikel ausgibt, die länger als 5 Zeichen sind. Verwende `continue`, um kürzere Artikel zu überspringen.  
 _Erwartete Ausgabe:_ `Banane Orange`

c) **(5 Punkte)** Erkläre den Unterschied zwischen `break` und `continue` und gib je ein sinnvolles Einsatzszenario an.

## Lösung

**a)**

```csharp
string[] artikel = { "Apfel", "Banane", "Kiwi", "Orange", "Mango" };

for (int i = 0; i < artikel.Length; i++)
{
    if (artikel[i] == "Kiwi")
        break;

    Console.WriteLine(artikel[i]);
}
```

**b)**

```csharp
string[] artikel = { "Apfel", "Banane", "Kiwi", "Orange", "Mango" };

for (int i = 0; i < artikel.Length; i++)
{
    if (artikel[i].Length <= 5)
        continue;

    Console.WriteLine(artikel[i]);
}
```

**c)**  
`break` beendet die gesamte Schleife sofort. Das ist sinnvoll, wenn ein Suchziel gefunden wurde und man nicht weiterlaufen muss. `continue` beendet nur den aktuellen Durchlauf und springt zum nächsten. Das ist sinnvoll, wenn bestimmte Elemente übersprungen werden sollen (z.B. ungültige oder zu kurze Einträge).

---

## Bewertung

| Teil               | Punkte |
| ------------------ | ------ |
| MC-Fragen (10 × 4) | 40     |
| Freitext (3 × 15)  | 45     |
| **Gesamt**         | **85** |

---

**Gutes Gelingen!**

---
