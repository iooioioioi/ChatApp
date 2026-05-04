using System;

namespace ChattCommon
{
    /// <summary>
    /// TODO: Implementera Message-klassen
    /// Ska representera ett meddelande i chatsystemet.
    /// Krav:
    /// - Properties: Sender (string), Content (string), Timestamp (DateTime)
    /// - Metod: ToString() - formaterad visning
    /// - Metod: Serialize() - konvertera till nätverksformat (SENDER|CONTENT|TIMESTAMP)
    /// - Statisk metod: Deserialize(string) - skapa från nätverksformat
    /// </summary>
    public class Message
    {
        public string Sender { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        public Message(string sender, string content)
        {
            Sender = sender ?? "Unknown";
            Content = content ?? "";
            Timestamp = DateTime.Now;
        }

        public override string ToString()
        {
            // Implementering: Visa tid, avsändare och innehål
            return $"[{Timestamp:HH:mm:ss}] {Sender}: {Content}";
        }

        public string Serialize()
        {
            // TODO: Format: SENDER|CONTENT|TIMESTAMP
            throw new NotImplementedException();
        }

        public static Message Deserialize(string data)
        {
            // TODO: Parsa från nätverksformat
            throw new NotImplementedException();
        }
    }
}
