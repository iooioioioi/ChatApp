using System;

namespace ChattCommon
{
    /// <summary>
    /// Representerar ett meddelande i chatsystemet.
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
            return $"[{Timestamp:HH:mm:ss}] {Sender}: {Content}";
        }

        /// <summary>
        /// Konverterar meddelandet till ett format som kan skickas över nätverket.
        /// Format: SENDER|CONTENT|TIMESTAMP
        /// </summary>
        public string Serialize()
        {
            return $"{Sender}|{Content}|{Timestamp:yyyy-MM-dd HH:mm:ss}";
        }

        /// <summary>
        /// Skapar ett Message-objekt från ett serialiserat strängformat.
        /// </summary>
        public static Message Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 3)
            {
                var message = new Message(parts[0], parts[1]);
                if (DateTime.TryParse(parts[2], out var timestamp))
                {
                    message.Timestamp = timestamp;
                }
                return message;
            }
            return new Message("System", data);
        }
    }
}
