using System;

namespace ChattCommon
{
    public class Message
    {
        public string Sender { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        public Message(string sender, string content)
        {
            Sender = sender ?? "Unknown";
            Content = content ?? string.Empty;
            Timestamp = DateTime.Now;
        }

        public override string ToString()
        {
            return $"[{Timestamp:HH:mm:ss}] {Sender}: {Content}";
        }

        public string Serialize()
        {
            return $"{Sender}|{Content}|{Timestamp:yyyy-MM-dd HH:mm:ss}";
        }

        public static Message Deserialize(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return new Message("System", string.Empty);

            var parts = data.Split('|');
            if (parts.Length < 3)
                return new Message("System", data);

            var message = new Message(parts[0], parts[1]);
            if (DateTime.TryParse(parts[2], out var ts))
                message.Timestamp = ts;

            return message;
        }
    }
}
