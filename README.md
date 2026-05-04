# ChattApp - Simpel ChattApp i C#

Början av en version av min ChattApp.

## Instruktioner

Implementera in följande ordning:

### 1. ChattCommon (Delad klassmodell)
- [ ] `Message.cs` - Representera meddelanden
- [ ] `User.cs` - Representera användaren
- [ ] `Logger.cs` - Loggning till fil för felsökning

### 2. ChattServer (Server)
- [ ] `ChatServer.cs` - Ska försöka på Ett Multitrådat serverprogram ()
- [ ] `ClientConnection.cs` - Individuell klienthantering

### 3. ChattClient (Klient)
- [ ] `ServerConnection.cs` - Klient & serverlogik
- [ ] `MainWindow.xaml` - Min WPF GUI layout
- [ ] `MainWindow.xaml.cs` - Code-behind logik (Hjälp av AI och Reddit Posts)

## Starta

```bash
cd ChattAppUpgift
dotnet build
dotnet run --project ChattServer
dotnet run --project ChattClient
```

## Mina Hjälpresurser

Se `docs/` för:
- `ClassDiagram.md` - Klassmodell diagram med hjälp av AI
- `Implementation.md` - Teknisk vägledning på hur jag ska bygga ihop appen från inget till en Komplett Chattapp (Skriven med hjälp av AI).
- `UserManual.md` - Användarguide för min app
- `ChatGPT + Gemini` - Hjälper till att lägga till kommentarer för min kod och hjälpa till hålla koll på vad som ska göras härnest. FÖr att effektivt jobba och slutföra mitt projekt.


## Lärandemål

Jag kommer att jobba på och lära dig:
- OOP (klasser, properties, metoder)
- Arv och polymorfism
- Nätverkskommunikation (TCP/Sockets)
- Multithreading
- GUI-utveckling (WPF)
- Asynkron programmering
- Filhantering
