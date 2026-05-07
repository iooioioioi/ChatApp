using System;
using System.IO;

namespace ChattCommon
{
    public class Logger
    {
        private readonly string _logPath;

        public Logger(string logFileName = "chatt.log")
        {
            _logPath = Path.Combine("logs", logFileName);
            Directory.CreateDirectory("logs");
        }

        public void Log(string message)
        {
            try
            {
                var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
                File.AppendAllText(_logPath, line + Environment.NewLine);
            }
            catch
            {
                // loggning ska inte krascha appen
            }
        }

        public void LogError(string message, Exception ex = null)
        {
            var text = $"ERROR: {message}";
            if (ex != null)
                text += $" - {ex.Message}";

            Log(text);
        }
    }
}
