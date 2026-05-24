using System;
using System.Net;

namespace ChattCommon
{
    public class Message
    {
        public string Sender { get; set; }
        public string Content { get; set; }
        public string ImageName { get; set; }
        public string ImageData { get; set; }
        public DateTime Timestamp { get; set; }

        public bool HasImage => !string.IsNullOrWhiteSpace(ImageData);

        public Message(string sender, string content, string imageName = null, string imageData = null)
        {
            Sender = string.IsNullOrWhiteSpace(sender) ? "Unknown" : sender;
            Content = content ?? string.Empty;
            ImageName = imageName ?? string.Empty;
            ImageData = imageData ?? string.Empty;
            Timestamp = DateTime.Now;
        }

        public override string ToString()
        {
            var result = $"[{Timestamp:HH:mm:ss}] {Sender}: {Content}";
            if (HasImage)
                result += $" [BILD: {ImageName}]";
            return result;
        }

        // gör texten till en enda rad som kan skickas över nätverket
        public string Serialize()
        {
            var safeSender = Uri.EscapeDataString(Sender);
            var safeContent = Uri.EscapeDataString(Content);
            var safeImageName = Uri.EscapeDataString(ImageName);
            var safeImageData = Uri.EscapeDataString(ImageData);
            return $"{safeSender}|{safeContent}|{Timestamp:yyyy-MM-dd HH:mm:ss}|{safeImageName}|{safeImageData}";
        }

        public static Message Deserialize(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return new Message("System", string.Empty);

            var parts = data.Split('|');
            if (parts.Length < 3)
                return new Message("System", data);

            var sender = Uri.UnescapeDataString(parts[0]);
            var content = Uri.UnescapeDataString(parts[1]);
            var message = new Message(sender, content);

            if (parts.Length >= 4)
                message.ImageName = Uri.UnescapeDataString(parts[3]);

            if (parts.Length >= 5)
                message.ImageData = Uri.UnescapeDataString(parts[4]);

            if (DateTime.TryParse(parts[2], out var ts))
                message.Timestamp = ts;

            return message;
        }
    }
}
