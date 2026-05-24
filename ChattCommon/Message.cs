using System;
using System.Text.Json;

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
        public string ImageName { get; set; }
        public string ImageData { get; set; }

        public bool HasImage => !string.IsNullOrWhiteSpace(ImageData);

        public Message()
        {
            Sender = "System";
            Content = string.Empty;
            Timestamp = DateTime.Now;
        }

        public Message(string sender, string content)
        {
            Sender = sender ?? "Unknown";
            Content = content ?? string.Empty;
            Timestamp = DateTime.Now;
        }

        public Message(string sender, string content, string imageName, string imageData)
            : this(sender, content)
        {
            ImageName = imageName;
            ImageData = imageData;
        }

        public override string ToString()
        {
            var text = $"[{Timestamp:HH:mm:ss}] {Sender}: {Content}";
            if (HasImage)
            {
                text += $" [Bild: {ImageName}]";
            }
            return text;
        }

        public string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }

        public static Message Deserialize(string data)
        {
            try
            {
                var message = JsonSerializer.Deserialize<Message>(data);
                return message ?? new Message("System", data);
            }
            catch
            {
                return new Message("System", data);
            }
        }
    }
}
