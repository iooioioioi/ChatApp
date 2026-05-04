# 📋 Dokumentations-Checklist för ChattApp

Baserat på projektuppgiften: **Slutleverans - Vad du ska lämna in**

---

## ✔ 1. KLASSDIAGRAM

**Fil**: `docs/ClassDiagram.md` (eller bild)

**Måste innehålla**:
- [ ] Alla klasser (Message, User, Logger, ChatServer, ClientConnection, ServerConnection, MainWindow)
- [ ] Properties för varje klass
- [ ] Metoder för varje klass
- [ ] Relationer mellan klasser (använder, arver, håller)
- [ ] Namespace-indelning (ChattCommon, ChattServer, ChattClient)
- [ ] Designmönster som används (Observer Pattern, Threading, etc.)

**Exempel struktur**:
```
ChattCommon
├── Message
│   ├── + Sender: string
│   ├── + Content: string
│   ├── + Timestamp: DateTime
│   ├── + ToString(): string
│   ├── + Serialize(): string
│   └── + Deserialize(string): Message
├── User
│   ├── + Username: string
│   ├── + Id: int
│   ├── + ConnectedTime: DateTime
│   └── + ToString(): string
└── Logger
    ├── - _logPath: string
    ├── + Log(string): void
    └── + LogError(string, Exception): void
```

---

## ✔ 2. GITHUB REPOSITORY MED COMMIT-HISTORIK

**Krav**:
- [ ] Repository initialiserat med `git init`
- [ ] `.gitignore` fil (bin/, obj/, etc.)
- [ ] Minst 3-4 meningsfulla commits
- [ ] Commit-meddelanden på svenska
- [ ] Varje commit har tydlig beskrivning av vad som implementerades

**Exempelvisa commits**:
```
1. "Init: Projektstruktur och klassmodell (Message, User, Logger)"
2. "Feature: Multitrådat server med klienthantering"
3. "Feature: WPF GUI-klient med nätverkskommunikation"
4. "Docs: Klassdiagram och användardokumentation"
```

**Visar**:
- [ ] Utvecklingsprocessen steg-för-steg
- [ ] Att du kan använda Git
- [ ] Ansvar och arbetsprocess

---

## ✔ 3. DOKUMENTATION (Huvudsaklig)

### 3.1 **README.md** (Projekt-överblick)

**Måste innehålla**:
- [ ] Projektnamn och kort beskrivning (1-2 meningar)
- [ ] Systemkrav (.NET version, etc.)
- [ ] **Snabbstart** (hur man startar server och klient)
- [ ] Mappstruktur (kort)
- [ ] Teknologier som används (C#, WPF, TCP-sockets)
- [ ] Länk till mer dokumentation

**Exempel**:
```markdown
# ChattApp - Distribuerad Chattapplikation

En chattserver och klient utvecklad i C# med WPF GUI.

## Krav
- .NET 6.0+
- Windows för GUI

## Snabbstart
\`\`\`bash
dotnet run --project ChattServer
dotnet run --project ChattClient
\`\`\`

## Struktur
- ChattServer/ - TCP-server
- ChattClient/ - WPF GUI-klient
- ChattCommon/ - Delad klassmodell
```

---

### 3.2 **KLASSDIAGRAM & DESIGN** (Teknisk dokumentation)

**Fil**: `docs/ClassDiagram.md` + `docs/Design.md`

**Måste innehålla**:
- [ ] Alla klassers namn, properties, metoder
- [ ] Nätverksprotocholl (format för meddelanden)
- [ ] Arkitektur-diagram (Server ↔ Klient)
- [ ] Designmönster som användes
  - [ ] Threading (varje klient i egen tråd)
  - [ ] Async/Await (responsiv UI)
  - [ ] Observer Pattern (events)
  - [ ] Dependency Injection (Logger)

**Exempel för nätverksprotokoll**:
```
CLIENT → SERVER
[USERNAME]\n              (initial handshake)
[MESSAGE]\n               (user messages)

SERVER → CLIENT
SENDER|CONTENT|TIME\n     (formatted messages)
```

---

### 3.3 **IMPLEMENTATIONS-GUIDE** (för utvecklare)

**Fil**: `docs/Implementation.md`

**Måste innehålla**:
- [ ] Hur servern fungerar (step-by-step)
  - [ ] TcpListener på port 5000
  - [ ] AcceptTcpClient() loop
  - [ ] HandleClient() i egen tråd
  - [ ] BroadcastMessage() till alla klienter
  - [ ] Thread-safety med locks
  
- [ ] Hur klienten fungerar
  - [ ] Asynkron anslutning
  - [ ] Meddelandemottagning i bakgrund
  - [ ] UI-uppdateringar med Dispatcher
  - [ ] Event-hantering

- [ ] Klassernas ansvar (kort)

**Exempel**:
```markdown
## Server Architecture

1. **Start()** - Startar TcpListener på port 5000
2. **HandleClient()** - Körs i egen tråd för varje klient
3. **BroadcastMessage()** - Skickar meddelande till alla
4. **Thread-Safety** - Använder lock för _clients lista

## Message Flow
1. Klient A skickar meddelande
2. Server mottar via HandleClient()
3. BroadcastMessage() skickar till alla
4. Klient B mottar via ReceiveMessagesAsync()
```

---

### 3.4 **ANVÄNDAR-MANUAL** (för slutanvändare)

**Fil**: `docs/UserManual.md`

**Måste innehålla**:
- [ ] Systemkrav
- [ ] Installation & Start (steg-för-steg)
- [ ] GUI-beskrivning med bilder/layout
- [ ] Hur man använder systemet (ansluta, chatta, koppla ifrån)
- [ ] Vanliga fel och lösningar
- [ ] Exempel på en session

**Exempel**:
```markdown
## Starta Servern
1. Öppna terminal
2. Navigera till ChattApp
3. `dotnet run --project ChattServer`
4. Se "Servern lyssnar på port 5000..."

## Starta Klient
1. Öppna nytt terminal-fönster
2. `dotnet run --project ChattClient`
3. GUI öppnas
4. Ange användarnamn (ex. "Alice")
5. Klicka "Anslut"
6. Skriv meddelande och skicka
```

---

### 3.5 **TESTPLAN & RESULTAT** (Validering)

**Fil**: `docs/TestPlan.md`

**Måste innehålla**:
- [ ] Minst 5-10 testfall
- [ ] Vad varje test testar
- [ ] Förväntade resultat
- [ ] Faktiska resultat (✓ eller ✗)
- [ ] Stress-test (många meddelanden)
- [ ] Felhantering-tester

**Exempeltester**:
```markdown
## Test 1: Serverstart
- Kör: dotnet run --project ChattServer
- Förväntat: "Servern lyssnar på port 5000..."
- Resultat: ✓

## Test 2: Anslutning
- Starta server
- Starta klient
- Ange användarnamn och klicka "Anslut"
- Förväntat: Status visar "✓ Ansluten"
- Resultat: ✓

## Test 3: Meddelande
- Med två anslutna klienter
- Klient 1 skickar "Hej!"
- Förväntat: Klient 2 ser "[10:30:45] Klient1: Hej!"
- Resultat: ✓
```

---

## ✔ 4. UTVÄRDERING - FUNKTIONER & PRESTANDA

**Fil**: `docs/Evaluation.md` (eller egen fil)

**Måste innehålla**:

### 4.1 **Funktionell Utvärdering**
- [ ] Vilka funktioner är implementerade?
  - [ ] ✓ Server hanterar flera klienter
  - [ ] ✓ Broadcast-meddelanden
  - [ ] ✓ GUI för chatning
  - [ ] ✓ Loggning
  - [ ] Osv...

- [ ] Vad fungerar bra?
- [ ] Vad kunde förbättras?
- [ ] Vilka funktioner är inte implementerade?

### 4.2 **Prestanda-Utvärdering**
- [ ] **Minneskonsumption**: ~1 MB per klient
- [ ] **CPUanvändning**: Låg (events-driven)
- [ ] **Meddelandelatens**: <100ms
- [ ] **Max klienter**: Testat med X klienter
- [ ] **Stabilitet**: Körs utan crash

### 4.3 **Kodkvalitet**
- [ ] OOP-koncept demonstrerade (Inkapsling, Arv, Polymorfism)
- [ ] Thread-safety implementerad
- [ ] Felhantering robust
- [ ] Kod läsbar och kommenterad
- [ ] Tydlig separation of concerns

**Exempel**:
```markdown
# Utvärdering - ChattApp

## Funktionalitet
✓ Server hanterar 100+ klienter samtidigt
✓ Broadcast fungerar korrekt
✓ Loggning sparar all aktivitet
✓ GUI är responsiv
✓ UTF-8 stödjer åäö

## Prestanda
- Meddelandelatens: ~50ms lokalt
- Minneskonsumption: ~0.5MB per klient
- CPU: ~5% idle, <20% vid aktiv chatting

## Förbättringar
- Kunde implementera privatmeddelanden
- Kunde kryptera meddelanden
- Kunde lägga till user-list

## Slutsats
Systemet är fullt funktionellt och demonstrerar solid OOP-kunskap.
```

---

## ✔ 5. UTVECKLINGSDAGBOK (Valfritt, men rekommenderat)

**Fil**: Commits eller `docs/DevelopmentLog.md`

**Kan innehålla**:
- [ ] Vad du gjorde varje dag
- [ ] Problem du stötte på
- [ ] Hur du löste dem
- [ ] Lärande-punkter

---

---

# 📝 CHECKLIST FÖR SLUTLEVERANS

**Du måste skapa:**
- [ ] **README.md** - Överblick och snabbstart
- [ ] **docs/ClassDiagram.md** - Klassmodell
- [ ] **docs/Design.md** (eller Implementation.md) - Teknisk arkitektur
- [ ] **docs/UserManual.md** - Användarguide
- [ ] **docs/TestPlan.md** - Tester och resultat
- [ ] **docs/Evaluation.md** - Utvärdering av funktioner & prestanda
- [ ] **Git-repository** - Med 3-4 meningsfulla commits
- [ ] **Server & Klient** - Fungerade program

---

# 🎯 SNABB SAMMANFATTNING

| Dokumentation | Vad Det Är | Vad Det Måste Innehålla |
|---|---|---|
| **README.md** | Projektöverblick | Snabbstart, krav, struktur |
| **ClassDiagram.md** | Klassmodell | Alla klasser, properties, metoder |
| **Design.md** | Teknisk arkitektur | Protocol, threading, designmönster |
| **UserManual.md** | Användarguide | Steg-för-steg hur man använder |
| **TestPlan.md** | Test & resultat | 5-10 tester med resultat |
| **Evaluation.md** | Prestanda & kvalitet | Fungerar? Hur bra? Förbättringar? |
| **Git** | Version control | 3-4 commits med bra meddelanden |

---

**Tips**: Börja med **ClassDiagram.md** och **Design.md** först (fokus på arkitektur), sedan **README.md**, **UserManual.md**, **TestPlan.md** och **Evaluation.md**.
