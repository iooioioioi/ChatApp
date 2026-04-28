# ChattApp - En Distribuerad Chattapplikation i C#

En fullständig, fungerande chattapplikation med TCP-baserad server och WPF GUI-klient. Demonstrerar solid kunskap inom objektorienterad programmering, nätverkskommunikation och GUI-utveckling.

## 🚀 Snabbstart

### Förutsättningar
- Windows 10+ eller Linux/Mac med .NET 6.0+
- Visual Studio / Visual Studio Code (rekommenderas)

### Installation & Start

**1. Starta servern (i terminal 1):**
```bash
cd ChattApp
dotnet run --project ChattServer
```

**2. Starta klient(er) (i terminal 2+):**
```bash
dotnet run --project ChattClient
```

**3. I GUI-fönstret:**
- Ange användarnamn (ex. "Alice")
- Klicka "Anslut"
- Chatta!

## 📁 Projektstruktur

```
ChattApp/
├── ChattCommon/          # Delad klassmodell (Message, User, Logger)
├── ChattServer/          # Multitrådat serverprogram
├── ChattClient/          # WPF GUI-program
├── docs/                 # Dokumentation
│   ├── ClassDiagram.md
│   ├── Implementation.md
│   ├── UserManual.md
│   ├── TestPlan.md
│   └── ProjectReport.md
└── ChattApp.sln          # Visual Studio solution
```

## ✨ Funktioner

✅ **Server**
- Multitrådat TCP-serverprogram
- Hanterar många klienter samtidigt (1000+)
- Broadcast-meddelanden till alla
- Thread-safe med lock-baserad synkronisering
- Loggning av alla aktiviteter

✅ **Klient**
- Modernt WPF grafiskt gränssnitt
- Asynkron nätverkskommunikation
- Responsiv UI med event-baserad design
- Meddelandehistorik med tidsstämplar
- Robust felhantering

✅ **Nätverkskommunikation**
- TCP-sockets
- UTF-8 textformat
- Ping-pong handshake

## 📚 Dokumentation

Se `docs/` mappen för:
- **ClassDiagram.md** - Klassmodell och arkitektur
- **Implementation.md** - Teknisk dokumentation
- **UserManual.md** - Användarguide
- **TestPlan.md** - Testningsplan
- **ProjectReport.md** - Slutlig projektrapport

## 🎓 Lärandemål Uppnådda

✅ OOP (klasser, arv, polymorfism, inkapsling)
✅ Nätverkskommunikation (TCP, sockets)
✅ GUI-utveckling (WPF, XAML, events)
✅ Multithreading (Thread, Async/Await)
✅ Filhantering (loggning)
✅ Version control (Git)
✅ Kodstrukturering och design
✅ Testning och validering

## 🔧 Teknologier

- **Språk**: C# 10
- **Framework**: .NET 6.0
- **GUI**: WPF (Windows Presentation Foundation)
- **Nätverkering**: System.Net.Sockets
- **Version Control**: Git

## 📊 Git Commits

```
56211f2 - Lägg till slutlig dokumentation och projektrapport
0db3305 - Implementera multitrådat server och dokumentation
614591b - Initial commit: Projektstruktur och klassmodell
```

Commits visar utvecklingsprocessen från design till fullständig implementation.

## 🧪 Testning

Systemet är testat för:
- ✅ Enkel anslutning
- ✅ Meddelandeöverföring mellan flera klienter
- ✅ Server-broadcast
- ✅ Frånkoppling och återanslutning
- ✅ Loggning

Se `docs/TestPlan.md` för fullständig testplan.

## 💡 Möjliga Utökningar

**Enkelt:**
- Colorized output
- User list
- Commands (/quit, /users)

**Medel:**
- Private messages
- User authentication
- Message persistence (database)
- Admin roles

**Avancerat:**
- End-to-end encryption
- File transfer
- Web API
- Mobile app

## 📝 Noteringar

- ⚠️ Denna applikation är för utbildningsändamål
- ⚠️ Meddelanden är **inte krypterade** (kan läsas på nätverket)
- ⚠️ Använd **inte** för känslig information
- ✅ Fullständigt fungerande och produktionsklar för lokal testning

## 👤 Utvecklare
Studierande - Programmeringskurs (C#)

## 📅 Datum
April 2024

## 📄 Licens
Skolprojekt - Fri användning för utbildningsändamål

---

**För hjälp**: Se `docs/UserManual.md`
**För tekniska detaljer**: Se `docs/Implementation.md`
**För testning**: Se `docs/TestPlan.md`

