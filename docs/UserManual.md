# Användarmanual - ChattApp

## Systemkrav
- Windows 10 eller senare
- .NET 6.0 Runtime eller SDK
- Internetanslutning (eller localhost för lokal testning)

## Installation

### Från terminalen
```bash
# Klona eller navigera till projektmappen
cd ChattApp

# Bygg projektet
dotnet build

# Eller direkt kör
dotnet run --project ChattServer
dotnet run --project ChattClient
```

## Starta Servern

### Windows
```bash
dotnet run --project ChattServer
```

Du bör se:
```
╔════════════════════════════════╗
║   CHATT-SERVER STARTAS...      ║
╚════════════════════════════════╝

Servern lyssnar på port 5000...
```

**Servern måste köra innan klienter kan ansluta!**

## Starta Klienten

### Windows
```bash
dotnet run --project ChattClient
```

GUI-fönstret öppnas med anslutningsinställningar.

## Gränssnitt - Steg för steg

### 1. Anslutningsskärmen
```
┌─────────────────────────────┐
│ Server:        [localhost]  │
│ Port:          [5000]       │
│ Användarnamn:  [Ditt namn]  │
│                [Anslut]     │
└─────────────────────────────┘
```

**Instruktioner:**
- **Server**: Adressamn till servern (default: `localhost` för lokal server)
- **Port**: Portnummer (default: `5000`)
- **Användarnamn**: Ditt visningsnamn i chatten (ex. "Alice", "Bob")

### 2. Ansluta
1. Fyll i de tre fälten
2. Klicka knappen **"Anslut"**
3. Du bör se **"✓ Ansluten till servern"** i grönt överst

### 3. Chatten

```
┌──────────────────────────────────┐
│ Meddelandehistorik               │
│                                  │
│ [10:30:45] Alice: Hej!           │
│ [10:30:50] Bob: Hej själv!       │
│ [10:30:52] Alice: Hur mår du?    │
│                                  │
├──────────────────────────────────┤
│ [Skriv här.......] [Skicka]      │
└──────────────────────────────────┘
```

**Instruktioner:**
1. Skriv ditt meddelande i textfältet
2. Klicka **"Skicka"** eller tryck **Enter**
3. Meddelandet visas för alla anslutna användare

### 4. Frånkoppling
- Klicka knappen **"Koppla från"** för att koppla från servern
- Du kan ansluta igen senare med ett nytt användarnamn

## Exempel på Session

### Terminal 1 (Server)
```
╔════════════════════════════════╗
║   CHATT-SERVER STARTAS...      ║
╚════════════════════════════════╝

Servern lyssnar på port 5000...
Klient ansluten: Alice
Klient ansluten: Bob
[10:30:55] Alice: Hej världen!
[10:30:57] Bob: Tja!
```

### Terminal 2 (Klient - Alice)
```
✓ Ansluten till servern som Alice

Server: localhost   Port: 5000   Användarnamn: Alice   [Anslut]
```

GUI visar:
```
[10:30:55] System: Alice har anslutit sig.
[10:30:57] System: Bob har anslutit sig.
[10:30:55] Alice: Hej världen!
[10:30:57] Bob: Tja!
```

## Vanliga Problem

### "Anslutningsfel: Det går inte att ansluta till servern"
**Lösning:**
- Kontrollera att servern körs
- Verifiera att serveradresssen är korrekt (default: localhost)
- Verifiera att portnumret är rätt (default: 5000)
- Om du ansluter från en annan dator, använd serverns IP-adress istället för "localhost"

### "Du är inte ansluten till servern"
**Lösning:**
- Klicka "Anslut" igen
- Kontrollera anslutningsstatus (grönt = ansluten, rött = frånkopplad)

### Meddelanden visas inte
**Lösning:**
- Se till att du är ansluten (grön status)
- Kontrollera att du kan se andra användares meddelanden
- Om bara dina meddelanden saknas, kontrollera att du inte har blockerat dem

### Servern kraschar
**Lösning:**
- Starta servern igen
- Kontrollera loggfilen: `logs/server.log`

## Loggfiler

Alla transaktioner sparas i loggfiler:
- `logs/server.log` - Serverlogg
- `logs/client.log` - Klientlogg (skapas per klient-säsion)

Du kan öppna dessa i Notepad för att se detaljer om problem.

## Säkerhet & Tips

⚠️ **Observera:**
- Denna applikation är för utbildningsändamål
- Meddelanden är INTE krypterade (kan läsas av andra på nätverket)
- Använd INTE för känslig information
- Password-autentisering är inte implementerad

## Avancerad Användning

### Köra på en annan server
```bash
# Vid anslutning, använd datorns IP-adress istället för localhost
Server: 192.168.1.100
Port: 5000
```

### Ändra port
Du behöver redigera källkoden:
- Öppna `ChattServer/ChatServer.cs`
- Ändra `const int PORT = 5000;` till önskad port

### Köra flera servrar
Varje server måste ha en unik port:
- Modifiera `PORT` konstanten i `ChatServer.cs`
- Starta flera serverinstanser på olika portar

