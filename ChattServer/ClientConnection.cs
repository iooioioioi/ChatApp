using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using ChattCommon;

namespace ChattServer
{
    /// <summary>
    /// Representerar en klient-anslutning på servern.
    /// Hanterar läsning och skrivning av meddelanden.
    /// </summary>
    public class ClientConnection
    {
        private readonly TcpClient _tcpClient;
        private readonly NetworkStream _stream;
        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;

        public User User { get; private set; }

        public ClientConnection(TcpClient tcpClient, int clientId)
        {
            _tcpClient = tcpClient;
            _stream = tcpClient.GetStream();

            // UTF8 för att stödja åäö och andra tecken
            _reader = new StreamReader(_stream, Encoding.UTF8);
            _writer = new StreamWriter(_stream, Encoding.UTF8)
            {
                AutoFlush = true
            };

            // Läs användarnamn från klienten
            string username = _reader.ReadLine() ?? $"User{clientId}";
            User = new User(username, clientId);
        }

        /// <summary>
        /// Läser ett meddelande från klienten.
        /// Returnerar null om klienten kopplade från.
        /// </summary>
        public string ReadMessage()
        {
            try
            {
                return _reader.ReadLine();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Skickar ett meddelande till klienten.
        /// </summary>
        public void SendMessage(Message message)
        {
            try
            {
                _writer.WriteLine(message.Serialize());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel vid skickning till {User.Username}: {ex.Message}");
            }
        }

        /// <summary>
        /// Stänger anslutningen.
        /// </summary>
        public void Close()
        {
            try
            {
                _writer?.Close();
                _reader?.Close();
                _stream?.Close();
                _tcpClient?.Close();
            }
            catch { }
        }
    }
}
