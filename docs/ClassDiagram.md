# Klassdiagram för ChattAppen
# Med Hjälp ifrån ChatGPT

## Arkitektur

```
                  ┌─────────────────────────────────┐
                  │    ChattCommon (Shared)          │
                  ├─────────────────────────────────┤
                  │ • Message                  │
                  │ • User                     │
                  │ • Logger                   │
                  └─────────────────────────────────┘
                          △                △
                          │                │
                  ┌───────┴───────┐    ┌───┴───────────┐
                  │               │    │               │
          ┌───────▼──────────┐  ┌─▼───────────────┐
          │  ChattServer     │  │  ChattClient    │
          ├──────────────────┤  ├─────────────────┤
          │ • ChatServer     │  │ • MainWindow    │
          │ • ClientConn     │  │ • ServerConn    │
          └──────────────────┘  └─────────────────┘
```

## Klassrelationer

```
ChattCommon::Message
├── Serialize() -> string
├── Deserialize() -> Message
└── ToString() -> string

ChattCommon::User
├── Username: string
├── Id: int
└── ConnectedTime: DateTime

ChattCommon::Logger
├── Log(string)
└── LogError(string, Exception)

ChattServer::ChatServer
├── _clients: List<ClientConnection>
├── Start()
├── HandleClient(TcpClient)
└── BroadcastMessage(Message)

ChattServer::ClientConnection
├── _tcpClient: TcpClient
├── User: User
├── ReadMessage()
├── SendMessage(Message)
└── Close()

ChattClient::ServerConnection
├── _client: TcpClient
├── MessageReceived: event
├── ConnectAsync()
├── SendMessage(string)
└── Disconnect()

ChattClient::MainWindow
├── _connection: ServerConnection
├── ConnectButton_Click()
├── SendButton_Click()
└── OnMessageReceived()
```

## Designmönster

1. **Observer Pattern**: Events används för meddelandemottagning
2. **Threading**: Varje klient körs i egen tråd
3. **Dependency Injection**: Logger passeras till konstruktorer
4. **Async/Await**: Non-blocking UI
5. **Komposition**: Preferences framför arv
