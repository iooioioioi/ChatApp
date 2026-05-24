# Klassdiagram för ChattAppen

## Arkitektur

```
                  ┌─────────────────────────────────────────┐
                  │              ChattCommon                 │
                  ├─────────────────────────────────────────┤
                  │ • Message                                │
                  │ • User                                   │
                  │ • Logger                                 │
                  └─────────────────────────────────────────┘
                           △                 △
                           │                 │
                  ┌────────┴────────┐  ┌───────────────┴─────────────┐
                  │    ChattServer   │  │          ChattClient        │
                  ├──────────────────┤  ├─────────────────────────────┤
                  │ • ChatServer     │  │ • MainWindow                │
                  │ • ClientConnection│  │ • ServerConnection          │
                  └──────────────────┘  └─────────────────────────────┘
```

## Klassrelationer

```
ChattCommon::Message
├── Sender: string
├── Content: string
├── ImageName: string
├── ImageData: string
├── Timestamp: DateTime
├── HasImage: bool
├── Serialize() -> string
├── Deserialize(string) -> Message
└── ToString() -> string

ChattCommon::User
├── Username: string
├── Id: int
└── ConnectedTime: DateTime

ChattCommon::Logger
├── Log(string)
└── LogError(string, Exception)

ChattServer::ChatServer
├── _listener: TcpListener
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
├── ConnectionStatusChanged: event
├── ConnectAsync(string, int, string)
├── SendMessage(string)
├── SendImage(string, string)
└── Disconnect()

ChattClient::MainWindow
├── _connection: ServerConnection
├── ConnectButton_Click()
├── SendButton_Click()
├── ImageButton_Click()
├── MessageInput_KeyDown()
├── OnMessageReceived()
└── DisplayMessage(Message)
```

## Funktioner

- Servern tar emot flera klientanslutningar samtidigt.
- Servern tar emot och skickar vidare meddelanden i realtid.
- Klienten ansluter till servern med användarnamn.
- Klienten kan skicka textmeddelanden med Enter eller knappen Skicka.
- Klienten kan ladda upp bilder och visa dem i chattflödet.
- All trafik loggas till fil i både server och klient.

## Designmönster

1. **Observer Pattern**: `MessageReceived` och `ConnectionStatusChanged` används för UI-uppdateringar.
2. **Konkurrens**: Servern hanterar varje klient i sin egen tråd.
3. **Async/Await**: Klienten tar emot meddelanden utan att blockera UI.
4. **Komposition**: Klient och server använder delade modeller från `ChattCommon`.
