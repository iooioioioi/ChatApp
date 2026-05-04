# IMPLEMENTATION GUIDE - ChattApp

Denna guide visar hur jag kommer att impementera / bygga upp min ChattApp steg för steg.

## Fas 1: ChattCommon (Klassmodell)

### 1.1 Message-klassen
**Syfte**: Representera ett meddelande i chatsystemet

**Properties som krävs**:
```csharp
public string Sender { get; set; }
public string Content { get; set; }
public DateTime Timestamp { get; set; }
```

**Konstruktor**:
```csharp
public Message(string sender, string content)
{
    Sender = sender ?? "Unknown";
    Content = content ?? "";
    Timestamp = DateTime.Now;
}
```

**Metoder**:
- `ToString()` - Format: `[10:30:45] Alice: Hej världen`
- `Serialize()` - Format: `Alice|Hej världen|2024-04-28 10:30:45`
- `Deserialize(string data)` - Statisk metod som skapar Message från serialiserat format

### 1.2 User-klassen
**Syfte**: Representera en användare

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
**Syfte**: Centraliserad loggning

**Privat field**:
```csharp
private readonly string _logPath;
```

**Konstruktor**:
- Ta `logFileName` parameter (default "chatt.log")
- Sätt `_logPath = Path.Combine("logs", logFileName)`
- Skapa logs-mappen: `Directory.CreateDirectory("logs")`

**Metoder**:
- `Log(string message)` - Skriver med tidsstämpel
- `LogError(string message, Exception ex = null)` - Skriver fel

---

## Fas 2: ChattServer (Servern)

### 2.1 ChatServer-klassen
**Syfte**: TCP-server som hanterar många klienter

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
1. Skapa TcpListener på localhost:5000
2. Starta lyssnandet
3. Logga "SERVER STARTAD"
4. Loop som accepterar TcpClient
5. För varje client, starta ny Thread som kallar HandleClient()

**HandleClient() metod** (körs i egen tråd):
1. Skapa ny ClientConnection
2. Lägg till i _clients (med lock för thread-safety)
3. Lägg meddelande i log
4. Loop som läser meddelanden från klienten
5. Broadcast varje meddelande till alla
6. Vid disconnect: ta bort från lista och notifiera andra

**BroadcastMessage() metod**:
1. För varje ClientConnection i _clients (med lock)
2. Anrop SendMessage() på varje client
3. Fånga exceptions för frånkopplade klienter

### 2.2 ClientConnection-klassen
**Syfte**: Representerar en enskild klientanslutning på servern

**Privata fields**:
```csharp
private readonly TcpClient _tcpClient;
private readonly NetworkStream _stream;
private readonly StreamReader _reader;
private readonly StreamWriter _writer;
public User User { get; private set; }
```

**Konstruktor**:
1. Spara _tcpClient
2. Hämta NetworkStream
3. Skapa StreamReader och StreamWriter med UTF-8
4. Läs användarnamn från första raden
5. Skapa User-objekt

**ReadMessage()**:
- Returnerar `_reader.ReadLine()`
- Returnerar null om felkod

**SendMessage()**:
- Konvertera Message.Serialize()
- Skriv via StreamWriter

**Close()**:
- Stäng reader, writer, stream, client

---

## Fas 3: ChattClient (Klienten)

### 3.1 ServerConnection-klassen
**Syfte**: Hanterar kommunikation med servern från klienten

**Privata fields**:
```csharp
private TcpClient _client;
private NetworkStream _stream;
private StreamReader _reader;
private StreamWriter _writer;
private readonly Logger _logger;
private string _username;
private bool _isRunning = false;
```

**Events**:
```csharp
public event Action<Message> MessageReceived;
public event Action<string> ConnectionStatusChanged;
```

**Property**:
```csharp
public bool IsConnected => _client?.Connected == true;
```

**ConnectAsync()**:
1. Validera input
2. Skapa TcpClient
3. Anslut med timeout (5 sekunder)
4. Skapa reader/writer
5. Skicka användarnamn
6. Sätt _isRunning = true
7. Starta ReceiveMessagesAsync() i bakgrunden

**ReceiveMessagesAsync()**:
1. Loop medan _isRunning
2. Läs meddelanden från servern
3. Deserialize och anrop MessageReceived event
4. Vid error: anrop Disconnect()

**SendMessage()**:
1. Validera att _client.Connected
2. Skriv meddelande via StreamWriter

**Disconnect()**:
1. Sätt _isRunning = false
2. Stäng alla connections
3. Anrop ConnectionStatusChanged event

### 3.2 MainWindow XAML
**Layout**:
- Row 0: Rubrik med ChattApp-title och status (grön/röd)
- Row 1: ScrollViewer med TextBlock för meddelandehistorik
- Row 2: TextBox för input + Send-button
- Row 3: Settings (Server, Port, Username) + Connect-button

### 3.3 MainWindow Code-Behind
**Fields**:
```csharp
private ServerConnection _connection;
```

**Constructor**:
1. InitializeComponent()
2. Skapa ServerConnection
3. Koppla events: MessageReceived, ConnectionStatusChanged

**ConnectButton_Click()**:
1. Om redan ansluten: Disconnect()
2. Validera input
3. Anrop ConnectAsync()
4. Uppdatera UI (enable/disable fields)

**SendButton_Click()**:
1. Hämta text från TextBox
2. Validera input
3. Anrop SendMessage()
4. Rensa TextBox

**OnMessageReceived()**:
1. Använd Dispatcher.Invoke() för UI-uppdateringar
2. Lägg till meddelande i MessageHistory TextBlock

**OnConnectionStatusChanged()**:
1. Uppdatera StatusText
2. Sätt färg (grön = ansluten, röd = frånkopplad)
3. Enable/disable controls

---

## Testning

1. **Starta servern** 
   ```bash
   dotnet run --project ChattServer
   ```

2. **Starta två klienter**
   ```bash
   dotnet run --project ChattClient
   ```

3. **Testa grundfunktion**:
   - Anslut första klient som "Alice"
   - Anslut andra klient som "Bob"
   - Skicka meddelande från Alice
   - Verifiera att Bob ser det

4. **Testa felhantering**:
   - Felaktig port
   - Inget användarnamn
   - Frånkoppling

---

## Tips & Tricks

- **Thread-safety**: Använd `lock (_lockObj)` när du accessar delad lista
- **UTF-8**: Använd `Encoding.UTF8` för att stödja åäö
- **Async**: Använd `await` och `async Task` för bättre responsivitet
- **UI-updates**: Använd alltid `Dispatcher.Invoke()` från annan tråd
- **Loggning**: Logga ofta för debugging

Lycka till! 🚀
