# Klassdiagram för ChattApp

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

## Klassmodell

### ChattCommon

#### Message
- **Ansvar**: Representerar ett meddelande i systemet
- **Attribut**:
  - `Sender: string` - Avsändarens namn
  - `Content: string` - Meddelandets innehål
  - `Timestamp: DateTime` - Tidpunkt när meddelandet skapades
- **Metoder**:
  - `ToString()` - Formaterad strängrepresentation
  - `Serialize()` - Konverterar till nätverksformat
  - `Deserialize(string)` - Skapar Message från nätverksformat

#### User
- **Ansvar**: Representerar en användar-anslutning
- **Attribut**:
  - `Username: string` - Användarnamn
  - `Id: int` - Unikt ID
  - `ConnectedTime: DateTime` - Anslutningsidpunkt
- **Metoder**:
  - `ToString()` - Strängrepresentation

#### Logger
- **Ansvar**: Loggar information till fil
- **Metoder**:
  - `Log(string)` - Skriver normalt meddelande
  - `LogError(string, Exception)` - Skriver felmeddelande

### ChattServer

#### ChatServer
- **Ansvar**: Huvudserver som hanterar alla klientanslutningar
- **Attribut**:
  - `_listener: TcpListener` - Lyssnar på inkommande anslutningar
  - `_clients: List<ClientConnection>` - Lista över anslutna klienter
  - `_logger: Logger` - Loggare för serverhändelser
- **Metoder**:
  - `Start()` - Startar servern och accepterar anslutningar
  - `HandleClient(TcpClient)` - Hanterar individuell klientanslutning (i tråd)
  - `BroadcastMessage(Message)` - Skickar meddelande till alla klienter

#### ClientConnection
- **Ansvar**: Representerar en enskild klientanslutning på servern
- **Attribut**:
  - `_tcpClient: TcpClient` - TCP-anslutningen
  - `_reader: StreamReader` - Läser data från klienten
  - `_writer: StreamWriter` - Skriver data till klienten
  - `User: User` - Användarinformation
- **Metoder**:
  - `ReadMessage()` - Läser ett meddelande från klienten
  - `SendMessage(Message)` - Skickar ett meddelande till klienten
  - `Close()` - Stänger anslutningen

### ChattClient

#### ServerConnection
- **Ansvar**: Hanterar klientanslutningen till servern
- **Attribut**:
  - `_client: TcpClient` - TCP-anslutningen
  - `_reader: StreamReader` - Läser data från servern
  - `_writer: StreamWriter` - Skriver data till servern
  - `MessageReceived: Event` - Event när meddelande tas emot
  - `ConnectionStatusChanged: Event` - Event när status ändras
- **Metoder**:
  - `ConnectAsync(host, port, username)` - Ansluter till servern
  - `ReceiveMessagesAsync()` - Läser meddelanden i background
  - `SendMessage(string)` - Skickar meddelande till server
  - `Disconnect()` - Kopplar från servern

#### MainWindow (WPF)
- **Ansvar**: Användarinterfacet
- **Komponenter**:
  - TextBlock för meddelandehistorik
  - TextBox för meddelande-input
  - Buttons för Skicka/Anslut
  - Status-display
- **Metoder**:
  - `ConnectButton_Click()` - Handlar anslutning/frånkoppling
  - `SendButton_Click()` - Handlar skickning av meddelande
  - `OnMessageReceived()` - Uppdaterar UI när meddelande tas emot
  - `OnConnectionStatusChanged()` - Uppdaterar anslutningsstatus

## Designmönster

1. **Observer Pattern**: `ServerConnection` använder events för att notifiera `MainWindow` om ändringar
2. **Singleton-liknande**: `Logger` används för centraliserad loggning
3. **Threading**: Server använder separate trådar för varje klient
4. **Async/Await**: Klienten använder async för responsivt UI

## Klassrelationer

- **ChatServer** använder **ClientConnection** (komposition)
- **ClientConnection** använder **User** (komposition)
- **ServerConnection** använder **Message** (komposition)
- **MainWindow** använder **ServerConnection** (komposition)
- Alla använder **Logger** (dependency injection)

## Arv och Polymorfism

I denna implementation fokuserar vi på enkel, läsbar kod:
- **Abstraktion**: Message.Serialize/Deserialize abstraherar formatet
- **Inkapsling**: Privata _reader/_writer field skyddas
- **Komposition**: Objects använder andra objects för functionality

## Flödete för ett meddelande

```
1. Användare skriver text i MainWindow
2. SendButton_Click() anropas
3. ServerConnection.SendMessage() skickar till server
4. ChatServer.HandleClient() mottar texten
5. ChatServer.BroadcastMessage() skickar till alla ClientConnection
6. ClientConnection.SendMessage() skickar till varje klient
7. ServerConnection.ReceiveMessagesAsync() mottar på klientens sida
8. OnMessageReceived event aktiveras
9. MainWindow uppdaterar MessageHistory TextBlock
```

