# Implementationsdokumentation - ChattApp

## Överblick
ChattApp är en fullständig distribuerad chattapplikation uppbyggd i C# med en TCP-baserad server och en WPF GUI-klient.

## Teknologier
- **Språk**: C# (.NET 6.0)
- **Server**: Multitrådat konsolprogram med `TcpListener`
- **Klient**: WPF (Windows Presentation Foundation) med XAML GUI
- **Kommunikation**: TCP-sockets med UTF-8 textformat (pipe-separerade meddelanden)
- **Loggning**: File-baserad loggning

## Serverarkitektur

### Nätverkskommunikation
- Servern lyssnar på `localhost:5000`
- `TcpListener` accepterar nya klientanslutningar
- Varje klient hanteras i en separat bakgrundstråd (`Thread`)

### Meddelanden Format
```
SENDER|CONTENT|TIMESTAMP
Exempel: Alice|Hej världen|2024-04-28 10:30:45
```

### Flöde för serverhantering
1. **Accept**: Server accepterar TcpClient
2. **Authenticate**: Läser användarnamn från klient (första raden)
3. **Loop**: Läser kontinuerligt meddelanden från klienten
4. **Broadcast**: Skickar varje meddelande till alla anslutna klienter
5. **Cleanup**: Vid frånkoppling, notifierar andra klienter och stänger resurserna

### Thread Safety
- `_clients` lista skyddas med `lock (_lockObj)` för att hålla den trådsäker
- Prevents race condition vid samtidiga anslutningar/frånkopplingar

## Klientarkitektur

### WPF GUI Layout
```
┌─────────────────────────────────┐
│  💬 ChattApp    [Status]        │  <- Rubrik med status
├─────────────────────────────────┤
│                                 │
│     Meddelandehistorik          │  <- ScrollViewer med TextBlock
│     (TextBlock med alla         │
│      meddelanden höglit)        │
│                                 │
├─────────────────────────────────┤
│ [Meddelande input....] [Skicka] │  <- Inputfält och skickabutton
├─────────────────────────────────┤
│ Server: [localhost] Port: [5000]│  <- Anslutningsinställningar
│ Användarnamn: [Name] [Anslut]   │
└─────────────────────────────────┘
```

### Asynkron kommunikation
- `ServerConnection.ConnectAsync()` - Ansluter utan att blockera UI
- `ReceiveMessagesAsync()` - Lyssnar på meddelanden i bakgrunden
- `Dispatcher.Invoke()` - Uppdaterar UI från bakgrundstråd (trådsäker)

### Events
- `MessageReceived` - Aktiveras när ett meddelande mottas
- `ConnectionStatusChanged` - Aktiveras när anslutningsstatus ändras

## Klassrelationer och Polymorfism

### Message-klassen
- Implementerar `Serialize()`/`Deserialize()` för nätverksöverföring
- `ToString()` för användarvänlig visning
- Inkapslar Sender, Content, Timestamp

### Logger-klassen (Singleton-like)
- Centraliserad loggning för både server och klient
- Skriver till `logs/server.log` och `logs/client.log`
- Thread-safe filskrivning

### Arv och Polymorfism
Projektet använder enkel, läsbar kod utan överflödig abstraktion:
- Fokus på **komposition** framför arv
- Clear **separation of concerns** mellan Server, Klient och Common
- Enkl **dependency injection** via konstruktorer

## Användarflöde

### 1. Starta server
```bash
dotnet run --project ChattServer
```
Server börjar lyssna och väntar på anslutningar.

### 2. Starta klient(er)
```bash
dotnet run --project ChattClient
```
GUI öppnas med anslutningsinställningar.

### 3. Anslut till server
1. Ange serveradress (default: localhost)
2. Ange port (default: 5000)
3. Ange användarnamn
4. Klicka "Anslut"

### 4. Chatta
- Skriv meddelande i textfältet
- Klicka "Skicka" eller tryck Enter
- Alla anslutna klienter mottar meddelandet

## Felhantering

### Server
- Fångar exceptions vid klienthantering utan att krascha
- Loggar alla fel till `logs/server.log`
- Notifierar andra klienter om frånkopplingar

### Klient
- Validerar användarinput innan anskutning
- Hanterar anslutningsfel med användarmeddelanden
- Reconnection möjlig via "Anslut"-knappen igen

## Loggning

### Server-logg (`logs/server.log`)
```
[2024-04-28 10:30:45] ===== SERVER STARTAD =====
[2024-04-28 10:30:50] INFO: Ny anslutning: Alice (ID: 1)
[2024-04-28 10:30:55] [10:30:55] Alice: Hej världen!
[2024-04-28 10:31:00] INFO: Klient frånkopplad: Alice
```

### Klient-logg (`logs/client.log`)
```
[2024-04-28 10:30:50] Ansluten till localhost:5000 som 'Alice'
[2024-04-28 10:30:55] Meddelande skickat: Hej världen!
```

## Prestandaöverväganden

1. **Threading**: Varje klient körs i en separat tråd, möjliggör tusentals samtidiga anslutningar
2. **Broadcast**: Alla klienter får alla meddelanden (O(n) komplexitet)
3. **GUI Responsivitet**: Async-programmering säkerställer att UI inte fryser
4. **Minimi-dependencies**: Endast .NET Standard bibliotek

## Utökningar och Förbättringar

För avancerade studentuppdrag kan man lägga till:
- **Privatmeddelanden**: Adressera specifika användare
- **Användartyper**: Admin, moderator med specialrättigheter
- **Kryptering**: Använd `System.Security.Cryptography`
- **Databaskoppling**: Lagra meddelanden i SQL Server
- **Emojis**: Stöd för Unicode-tecken
- **File transfer**: Skicka filer mellan klienter
- **Grupp-chattar**: Separate chat-rum
- **UI Themes**: Dark/Light mode

## Testning

### Manuell testning
1. Starta server
2. Starta flera klienter
3. Testa meddelandet flöde mellan klienter
4. Testa frånkoppling och återanslutning
5. Kontrollera loggfiler

### Automatiserad testning (för framtida)
Kan implementeras med NUnit eller xUnit:
- Unit tests för Message.Serialize/Deserialize
- Integration tests för server-klient kommunikation
- Stress tests för multiple simulerade klienter

