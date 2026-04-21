# Hausuebung 2026-04-15

Bearbeitet am: 21.04.2026

## 1) Free tiers aus dem *inference* Ordner in OpenCode konfigurieren
Status: Bereits erledigt.

Dokumentation (was gut geht / probleme):
- Funktioniert sehr gut: GitHub Copilot, OpenRouter, Hugging Face, Nvidia, Kimi For Coding.
- Problemfaelle: Groq und moonshot funktionieren aktuell nicht.
- Nachweis: Screenshot `SWPAIAPI.png` zeigt die eingerichteten Credentials aus `opencode auth list`.

Kurzfazit:
- Der groesste Teil der Free-Tier-Konfiguration laeuft stabil.
- Die beiden Ausnahmen (`groq`, `moonshot`) sind klar isoliert und koennen gezielt nachgeprueft werden (API-Key, Endpoint, Modellname, Quota/Rate-Limit).

## 2) Projekt 2025_Blazor ins Repo kopieren und zum Laufen bringen
Status: Erledigt.

Ablage im Repo:
- `2026-04-15/2025_Blazor`

Durchgefuehrte Checks:
- Build: `dotnet build BlazorApp1.sln` -> erfolgreich (0 Fehler, 0 Warnungen).
- Laufcheck: lokale HTTP-Antwort auf `http://127.0.0.1:5055` -> `HTTP_STATUS=200`.

Notwendige Anpassung fuer den Start:
- Datei: `2026-04-15/2025_Blazor/BlazorApp1/Program.cs`
- Aenderung: Default-Windows-EventLog-Provider deaktiviert und Console/Debug Logging gesetzt.
- Grund: Vorheriger Startfehler wegen fehlender Schreibrechte auf den Windows Event Log.

## 3) public-apis ansehen und 3 Lieblinge auswaehlen
Quelle: https://github.com/public-apis/public-apis

Meine 3 Favoriten:
1. Open-Meteo (Weather)
   - Warum: Kein API-Key noetig, schnell fuer Wetterdaten, sehr gut fuer Schul- und Webprojekte.
2. REST Countries (Open Data)
   - Warum: Laenderdaten (Namen, Flaggen, Sprachen, Regionen) sind ideal fuer UIs mit Filter/Suche.
3. Open Food Facts (Food & Drink)
   - Warum: Sehr viele echte Produktdaten; spannend fuer Barcode-, Ernaehrungs- oder Einkaufs-Apps.

## 4) Verbesserungs- / Aenderungsvorschlaege
Vorschlaege zur Arbeitsweise und Projektqualitaet:
1. Fuer jeden neuen Provider (z. B. Groq/moonshot) einen kleinen Smoke-Test mit Standardprompt dokumentieren.
2. Eine gemeinsame Fehlerliste im Repo fuehren (Provider, Fehlerbild, vermutete Ursache, naechster Test).
3. Beim Blazor-Projekt eine kurze `RUN.md` mit Startbefehl und typischen Problemen anlegen.
4. Fuer API-Auswahl immer 1 Fallback-API notieren, falls ein Dienst ausfaellt.

## 5) Traum-App bis Ende Semester
Arbeitstitel: GlobeBite

Idee:
- Eine Web-App, die Wetter, Laenderinfos und Lebensmitteldaten kombiniert.

Kernnutzen:
- Nutzer waehlen ein Land oder eine Stadt.
- Die App zeigt aktuelles Wetter (Open-Meteo) und passende Laenderinfos (REST Countries).
- Dazu koennen Produkte per Suche/Barcode geprueft werden (Open Food Facts), z. B. Zutaten oder Naehrwerte.

MVP bis Semesterende:
1. Suchseite fuer Land/Stadt.
2. Wetterkarte + Basis-Laenderprofil.
3. Produktsuche mit Detailansicht.
4. Favoritenliste mit lokalem Speichern im Browser.

Erweiterung (wenn Zeit bleibt):
- Empfehlungen fuer "passt heute zum Wetter" (z. B. warme/kalte Mahlzeiten) mit einfacher Regel-Logik.
