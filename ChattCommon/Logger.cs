using System;
using System.IO;

namespace ChattCommon
{
    /// <summary>
    /// Enkel loggarklass för att spara information till fil.
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

        /// <summary>
        /// Skriver ett meddelande till loggfilen.
        /// </summary>
        public void Log(string message)
        {
            try
            {
                var logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
                File.AppendAllText(_logPath, logLine + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel vid loggning: {ex.Message}");
            }
        }

        /// <summary>
        /// Skriver ett fel till loggfilen.
        /// </summary>
        public void LogError(string message, Exception ex = null)
        {
            var errorMsg = $"ERROR: {message}";
            if (ex != null)
                errorMsg += $" - {ex.Message}";
            Log(errorMsg);
        }
    }
}
