# ChattApp - En distribuerad chattapplikation i C#

## Projektöversikt
ChattApp är en chattapplikation med en server och en klient. Användare kan ansluta till servern och skicka meddelanden som distribueras till alla anslutna klienter.

## Systemaritektur
- **Server**: Multitrådat konsolprogram som lyssnar på port 5000
- **Klient**: WPF GUI-program som ansluter till servern
- **Klassmodell**: Använder arv, polymorfism och inkapsling

## Komma igång

### Krav
- .NET 6.0 eller senare
- Visual Studio 2022 eller Visual Studio Code

### Starta servern
```bash
dotnet run --project ChattServer/ChattServer.csproj
```

### Starta klienten
```bash
dotnet run --project ChattClient/ChattClient.csproj
```

## Klassmodell
Se `docs/ClassDiagram.md` för klassdiagram.

## Struktur
```
ChattApp/
├── ChattServer/          # Serverprogram
├── ChattClient/          # Klientprogram med WPF
├── ChattCommon/          # Delad kod (Message, User, etc.)
├── logs/                 # Loggfiler
└── docs/                 # Dokumentation
```

## Git-historik
Se commit-historiken för att följa utvecklingsprocessen.
