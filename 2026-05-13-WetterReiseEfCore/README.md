# Wetter-Reise EF Core Console

Hausuebung vom 2026-05-13:

- simples Console-Projekt mit EF Core
- SQLite-Datenbank `wetterreise.db`
- 2 Tabellen:
  - `Destinations`
  - `WeatherForecasts`
- CRUD-Operationen im Konsolenmenue

Start:

```powershell
dotnet run --project .\2026-05-13-WetterReiseEfCore\2026-05-13-WetterReiseEfCore.csproj
```

Beim ersten Start wird die Datenbank automatisch erstellt und mit zwei Beispiel-Reisezielen befuellt.
