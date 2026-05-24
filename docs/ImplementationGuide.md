# IMPLEMENTATION GUIDE - ChattApp

Denna guide beskriver hur projektet är uppbyggt och vilka funktioner som finns i det färdiga systemet.

## Fas 1: ChattCommon (Klassmodell)

### 1.1 Message-klassen
**Syfte**: Representera ett meddelande i chatsystemet.

**Properties som krävs**:
```csharp
public string Sender { get; set; }
public string Content { get; set; }
public string ImageName { get; set; }
public string ImageData { get; set; }
public DateTime Timestamp { get; set; }
public bool HasImage => !string.IsNullOrWhiteSpace(ImageData);
```

**Konstruktor**:
```csharp
public Message(string sender, string content, string imageName = null, string imageData = null)
{
    Sender = string.IsNullOrWhiteSpace(sender) ? "Unknown" : sender;
    Content = content ?? string.Empty;
    ImageName = imageName ?? string.Empty;
    ImageData = imageData ?? string.Empty;
    Timestamp = DateTime.Now;
}
```

**Metoder**:
- `ToString()` - Format: `[10:30:45] Alice: Hej världen` och lägger till `[BILD: filnamn]` om bild finns.
- `Serialize()` - Serialiserar hela objektet med fält för text och bild.
- `Deserialize(string data)` - Skapar `Message` från nätverksdata.

### 1.2 User-klassen
**Syfte**: Representera en användare.

**Properties**:
```csharp
public string Username { get; set; }
public int Id { get; set; }
public DateTime ConnectedTime { get; set; }
```

**Konstruktor**:
```csharp
public User(string username, int id)
{
    Username = username ?? "Anonymous";
    Id = id;
    ConnectedTime = DateTime.Now;
}
```

### 1.3 Logger-klassen
**Syfte**: Centraliserad loggning för server och klient.

**Privat field**:
```csharp
private readonly string _logPath;
```

**Konstruktor**:
- Tar `logFileName` som parameter.
- Skapar mappen `logs` om den saknas.
- Sparar logg till fil.

**Metoder**:
- `Log(string message)` - Skriver tidsstämplat meddelande.
- `LogError(string message, Exception ex = null)` - Skriver felinformation.

---

## Fas 2: ChattServer (Servern)

### 2.1 ChatServer-klassen
**Syfte**: TCP-server som tar emot flera klienter och skickar vidare meddelanden.

**Privata fields**:
```csharp
private TcpListener _listener;
private readonly List<ClientConnection> _clients = new();
private readonly Logger _logger;
private int _clientIdCounter = 1;
private readonly object _lockObj = new();
private const int PORT = 5000;
```

**Konstruktor**:
```csharp
public ChatServer()
{
    _logger = new Logger("server.log");
}
```

**Start() metod**:
1. Starta TcpListener på port 5000.
2. Logga att servern startar.
3. Acceptera klienter i en loop.
4. Starta en ny tråd för varje ansluten klient.

**HandleClient() metod** (körs i egen tråd):
1. Skapa `ClientConnection` för klienten.
2. Lägg till klienten i listan under lås.
3. Broadcasta att användaren anslutit.
4. Läs varje inkommande rad och `Deserialize` till `Message`.
5. Sätt rätt avsändare och tid.
6. Skicka vidare meddelandet till alla andra.
7. Vid disconnect: ta bort klient och notify andra.

**BroadcastMessage() metod**:
1. Iterera över alla anslutna klienter.
2. Skicka meddelandet till varje klient.
3. Ta bort klienter som inte längre svarar.

### 2.2 ClientConnection-klassen
**Syfte**: Representerar en anslutning från en klient.

**Privata fields**:
```csharp
private readonly TcpClient _tcpClient;
private readonly NetworkStream _stream;
private readonly StreamReader _reader;
private readonly StreamWriter _writer;
public User User { get; private set; }
```

**Konstruktor**:
1. Initiera nätverksströmmar.
2. Läs användarnamn från klienten.
3. Skapa `User`-objekt.

**ReadMessage()**:
- Läser en rad från klienten.
- Returnerar `null` vid fel.

**SendMessage()**:
- Skriver serialiserat `Message` till klienten.

**Close()**:
- Stänger alla resurser.

---

## Fas 3: ChattClient (Klienten)

### 3.1 ServerConnection-klassen
**Syfte**: Hantera anslutning, mottagning och sändning med servern.

**Privata fields**:
```csharp
private TcpClient _client;
private NetworkStream _stream;
private StreamReader _reader;
private StreamWriter _writer;
private readonly Logger _logger;
private string _username;
private bool _isRunning;
```

**Events**:
```csharp
public event Action<Message> MessageReceived;
public event Action<string> ConnectionStatusChanged;
```

**ConnectAsync()**:
1. Sätt användarnamn.
2. Anslut till servern med timeout.
3. Initiera reader och writer.
4. Skicka användarnamn första raden.
5. Starta bakgrundstråd för mottagning.

**ReceiveMessagesAsync()**:
1. Läs rader från servern.
2. `Deserialize` till `Message`.
3. Anropa `MessageReceived`.
4. Vid fel: `Disconnect()`.

**SendMessage()**:
1. Skapa `Message` med text.
2. Skicka serialiserat meddelande.

**SendImage()**:
1. Skapa `Message` med `ImageName` och `ImageData`.
2. Skicka bilddata som Base64.

**Disconnect()**:
1. Stäng alla nätverksresurser.
2. Anropa status-event.

### 3.2 MainWindow XAML
**Layout**:
- Överst: rubrik och anslutningsstatus.
- Mitten: ScrollViewer med `MessagePanel`.
- Nederst: knapp för bilduppladdning, textfält, skicka-knapp.
- Längst ner: inloggningsfält för användarnamn och anslut.

### 3.3 MainWindow Code-Behind
**Fields**:
```csharp
private readonly ServerConnection _connection;
```

**Constructor**:
1. Initiera komponenter.
2. Skapa `ServerConnection`.
3. Prenumerera på events från servern.

**ConnectButton_Click()**:
1. Validera användarnamn.
2. Anslut till `127.0.0.1:5000`.
3. Visa anslutningsstatus.

**SendButton_Click()**:
1. Hämta text från `MessageInput`.
2. Skicka via `SendMessage()`.
3. Rensa textfältet.

**MessageInput_KeyDown()**:
1. Tryck Enter skickar meddelandet ockå.

**ImageButton_Click()**:
1. Öppna filväljare.
2. Läs bilden som byte-array.
3. Skicka Base64-data till servern.

**OnMessageReceived()**:
1. Bygger upp chattbubblor i `MessagePanel`.
2. Visar text och bild vid behov.

---

## Testning

1. **Starta servern**
   ```bash
   dotnet run --project ChattServer
   ```

2. **Starta klienten**
   ```bash
   dotnet run --project ChattClient
   ```

3. **Testa funktioner**:
   - Anslut med användarnamn.
   - Skicka text med Enter eller knappen Skicka.
   - Ladda upp en bild.
   - Bekräfta att bilden visas i chatthistoriken.

4. **Verifiera serverkrav**:
   - Ta emot klientanslutningar.
   - Hantera flera användare.
   - Skicka vidare text och bildmeddelanden.
   - Logga information till fil.

---

## Tips & Tricks

- **Thread-safety**: Använd `lock (_lockObj)` när du accessar delad lista
- **UTF-8**: Använd `Encoding.UTF8` för att stödja åäö
- **Async**: Använd `await` och `async Task` för bättre responsivitet
- **UI-updates**: Använd alltid `Dispatcher.Invoke()` från annan tråd
- **Loggning**: Logga ofta för debugging

Lycka till! 🚀
