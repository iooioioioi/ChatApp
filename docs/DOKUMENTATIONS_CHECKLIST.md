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

---

### 3.4 **ANVÄNDAR-MANUAL** (för slutanvändare)

**Fil**: `docs/UserManual.md`

**Måste innehålla**:
- [ ] Systemkrav
- [ ] Installation & Start (steg-för-steg)
- [ ] GUI-beskrivning med layout
- [ ] Hur man använder systemet (ansluta, chatta, koppla ifrån)
- [ ] Vanliga fel och lösningar
- [ ] Exempel på en session

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

---

# 📝 SLUTLIG CHECKLIST

**Du måste skapa:**
- [ ] **README.md** - Överblick och snabbstart
- [ ] **docs/ClassDiagram.md** - Klassmodell
- [ ] **docs/Design.md** - Teknisk arkitektur
- [ ] **docs/UserManual.md** - Användarguide
- [ ] **docs/TestPlan.md** - Tester och resultat
- [ ] **docs/Evaluation.md** - Utvärdering av funktioner & prestanda
- [ ] **Git-repository** - Med 3-4 meningsfulla commits

---

# 🎯 SNABB REFERENS

| Dokumentation | Vad Det Måste Innehålla | Status |
|---|---|---|
| **README.md** | Snabbstart, krav, struktur | |
| **ClassDiagram.md** | Alla klasser, properties, metoder, relationer | |
| **Design.md** | Protocol, threading, designmönster | |
| **UserManual.md** | Steg-för-steg guide för slutanvändare | |
| **TestPlan.md** | 5-10 tester med resultat | |
| **Evaluation.md** | Funktioner, prestanda, kodkvalitet | |
| **Git** | 3-4 commits med meningsfulla meddelanden | |

