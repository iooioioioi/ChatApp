# Testplan - ChattApp

## Testning av Funktionalitet

### Test 1: Serverstart
**Mål**: Verifiera att servern startar korrekt
- [ ] Kör `dotnet run --project ChattServer`
- [ ] Verifiera att servern lyssnar på port 5000
- [ ] Verifiera att startmeddelandet visas

**Förväntat resultat**: ✓ Servern visas "Servern lyssnar på port 5000..."

---

### Test 2: Klientstart
**Mål**: Verifiera att klienten startar med GUI
- [ ] Kör `dotnet run --project ChattClient`
- [ ] Verifiera att GUI-fönstret öppnas
- [ ] Kontrollera att anslutningsfälten är synliga

**Förväntat resultat**: ✓ WPF GUI-fönster visas med anslutningsinställningar

---

### Test 3: Enkel anslutning
**Mål**: Verifiera att en klient kan ansluta
- [ ] Starta server
- [ ] Starta klient
- [ ] Ange användarnamn (ex. "Alice")
- [ ] Klicka "Anslut"
- [ ] Verifiera grön status "Ansluten till servern"

**Förväntat resultat**: 
- ✓ Status visar grönt
- ✓ Inputfälten blir aktiva
- ✓ Server-loggen visar "Ny anslutning: Alice"

---

### Test 4: Enkelt meddelande
**Mål**: Verifiera att ett meddelande kan skickas och mottas
- [ ] Med anslutad klient, skriv "Hej världen"
- [ ] Klicka "Skicka"
- [ ] Verifiera att meddelandet visas i historiken
- [ ] Server-loggen visar meddelandeöversändelsen

**Förväntat resultat**: 
- ✓ Meddelandet visas som "[HH:MM:SS] Alice: Hej världen"
- ✓ Meddelandet sparas i loggfilst

---

### Test 5: Flera anslutningar
**Mål**: Verifiera att flera klienter kan ansluta samtidigt
- [ ] Starta server
- [ ] Starta klient 1 ("Alice")
- [ ] Starta klient 2 ("Bob")
- [ ] Alice skickar meddelande
- [ ] Verifiera att Bob ser Alices meddelande

**Förväntat resultat**:
- ✓ Båda klienterna visar "Ansluten"
- ✓ Meddelanden distribueras till båda
- ✓ Server visar två anslutna klienter

---

### Test 6: Broadcast-funktion
**Mål**: Verifiera att broadcast fungerar (alla får alla meddelanden)
- [ ] Starta server
- [ ] Starta 3 klienter: Alice, Bob, Charlie
- [ ] Charlie skriv "Hej alla!"
- [ ] Verifiera att alla tre ser meddelandet

**Förväntat resultat**:
- ✓ Alla tre klienter visar Charlies meddelande
- ✓ Systemmeddelanden visas när nya klienter ansluter

---

### Test 7: Frånkoppling
**Mål**: Verifiera att frånkoppling fungerar korrekt
- [ ] Med två anslutna klienter
- [ ] En klient klickar "Koppla från"
- [ ] Verifiera att status blir röd
- [ ] Server visar avsaknaden
- [ ] Andra klienten ser systemmeddelande

**Förväntat resultat**:
- ✓ Frånkopplad klient visar "✗ Frånkopplad"
- ✓ Andra klienterna ser "[HH:MM:SS] System: [namn] har koppla från"

---

### Test 8: Loggning
**Mål**: Verifiera att loggfiler skapas korrekt
- [ ] Starta server och köra en session
- [ ] Kontrollera att `logs/server.log` finns
- [ ] Öppna filen och verifiera innehållet

**Förväntat resultat**:
- ✓ Loggfil skapas automatiskt
- ✓ Innehåller tidsstämpel och alla händelser

---

### Test 9: Felhantering - Felaktig port
**Mål**: Verifiera att klienten hanterar felaktig port
- [ ] Ange port som bokstäver (ex. "abc")
- [ ] Klicka "Anslut"
- [ ] Verifiera att ett felmeddelande visas

**Förväntat resultat**:
- ✓ MessageBox visar "Ogiltigt portnummer!"

---

### Test 10: Felhantering - Ingen användarnamn
**Mål**: Verifiera att användarnamn är obligatoriskt
- [ ] Lämna användarnamn tomt
- [ ] Klicka "Anslut"
- [ ] Verifiera att ett felmeddelande visas

**Förväntat resultat**:
- ✓ MessageBox visar "Ange ett användarnamn!"

---

## Stresstest

### Test 11: Många meddelanden
**Mål**: Verifiera prestanda med många meddelanden
- [ ] Två anslutna klienter
- [ ] En klient skickar 100 meddelanden snabbt
- [ ] Verifiera att alla mottas

**Förväntat resultat**:
- ✓ Alla meddelanden mottagen
- ✓ GUI förblir responsivt
- ✓ Loggfil uppdateras

---

### Test 12: Högre belastning
**Mål**: Verifiera server med många klienter
- [ ] Starta servern
- [ ] Starta 5-10 klienter
- [ ] Verifiera att alla kan ansluta

**Förväntat resultat**:
- ✓ Alla klienter ansluter framgångsr ikt
- ✓ Server fortsätter att fungera

---

## Testresultat Sammanfattning

| Test | Status | Kommentar |
|------|--------|-----------|
| 1. Serverstart | ☐ | |
| 2. Klientstart | ☐ | |
| 3. Anslutning | ☐ | |
| 4. Enkelt meddelande | ☐ | |
| 5. Flera klienter | ☐ | |
| 6. Broadcast | ☐ | |
| 7. Frånkoppling | ☐ | |
| 8. Loggning | ☐ | |
| 9. Felkod - port | ☐ | |
| 10. Felkod - namn | ☐ | |
| 11. Stresstest | ☐ | |
| 12. Belastningtest | ☐ | |

**Övergripande resultat**: ☐ Godkänd | ☐ Delvis godkänd | ☐ Ej godkänd

