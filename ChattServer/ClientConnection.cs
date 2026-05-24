using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using ChattCommon;

namespace ChattServer
{
    public class ClientConnection
    {
        private readonly TcpClient _tcpClient;
        private readonly NetworkStream _stream;
        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;

        public User User { get; private set; }

        // läser in användarnamnet som klienten skickar direkt efter anslutning
        // läser användarnamnet som klienten skickar när den ansluter
        public ClientConnection(TcpClient tcpClient, int clientId)
        {
            _tcpClient = tcpClient;
            _stream = tcpClient.GetStream();
            _reader = new StreamReader(_stream, Encoding.UTF8);
            _writer = new StreamWriter(_stream, Encoding.UTF8) { AutoFlush = true };

            var username = _reader.ReadLine();
            User = new User(string.IsNullOrWhiteSpace(username) ? $"User{clientId}" : username, clientId);
        }

        // läser ett meddelande från klienten
        // läser ett meddelande från klienten
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

        // skickar ett serialiserat meddelande till klienten
        public void SendMessage(Message message)
        {
            if (message == null)
                return;

            try
            {
                _writer.WriteLine(message.Serialize());
            }
            catch
            {
                // skicka fel ignoreras, klienten avslutas senare
            }
        }

        // stänger alla nätverksresurser för klienten
        public void Close()
        {
            try
            {
                _writer?.Close();
                _reader?.Close();
                _stream?.Close();
                _tcpClient?.Close();
            }
            catch
            {
                // inget mer att göra
            }
        }
    }
}
