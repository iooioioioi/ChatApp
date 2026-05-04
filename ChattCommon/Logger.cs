using System;
using System.IO;

namespace ChattCommon
{
    /// <summary>
    /// TODO: Implementera Logger-klassen
    /// Ska hantera loggning av meddelanden till fil.
    /// Krav:
    /// - Konstruktor: Logger(string logFileName = "chatt.log")
    /// - Metod: Log(string message) - skriver meddelande med tidsstämpel
    /// - Metod: LogError(string message, Exception ex = null) - skriver felmeddelande
    /// - Ska automatiskt skapa logs-mappen om den inte finns
    /// </summary>
    public class Logger
    {
        private readonly string _logPath;

        public Logger(string logFileName = "chatt.log")
        {
            _logPath = Path.Combine("logs", logFileName);

            // Skapa logs-mappen om den inte finns
            Directory.CreateDirectory("logs");
        }

        public void Log(string message)
        {
            // TODO: Lägg till tidsstämpel och skriv till fil
            throw new NotImplementedException();
        }

        public void LogError(string message, Exception ex = null)
        {
            // TODO: Formatera fel och skriv till logg
            throw new NotImplementedException();
        }
    }
}
