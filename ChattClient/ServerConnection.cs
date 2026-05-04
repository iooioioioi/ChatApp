using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChattCommon;

namespace ChattClient
{
    /// <summary>
    /// TODO: Implementera ServerConnection-klassen
    /// Hanterar anslutningen och kommunikationen med servern från klientsidan.
    /// Krav:
    /// - Privat TcpClient _client
    /// - Privat NetworkStream _stream
    /// - Privat StreamReader _reader
    /// - Privat StreamWriter _writer
    /// - Public event Action<Message> MessageReceived
    /// - Public event Action<string> ConnectionStatusChanged
    /// - Public property bool IsConnected
    /// - Metod: Task<bool> ConnectAsync(string host, int port, string username)
    /// - Metod: Task ReceiveMessagesAsync() - läser meddelanden i bakgrunden
    /// - Metod: void SendMessage(string content)
    /// - Metod: void Disconnect()
    /// </summary>
    public class ServerConnection
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;
        private readonly Logger _logger;
        private string _username;
        private bool _isRunning = false;

        public bool IsConnected { get; private set; }
        public event Action<Message> MessageReceived;
        public event Action<string> ConnectionStatusChanged;

        public ServerConnection()
        {
            _logger = new Logger("client.log");
        }

        public async Task<bool> ConnectAsync(string host, int port, string username)
        {
            try
            {
                _username = username;
                _client = new TcpClient();

                // TODO: Anslut med timeout
                // TODO: Initiera streams
                // TODO: Skicka användarnamn
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError("Anslutningsfel", ex);
                ConnectionStatusChanged?.Invoke($"✗ Anslutningsfel: {ex.Message}");
                return false;
            }
        }

        private async Task ReceiveMessagesAsync()
        {
            // TODO: Implementera
            throw new NotImplementedException();
        }

        public void SendMessage(string content)
        {
            // TODO: Implementera
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            // TODO: Implementera
            throw new NotImplementedException();
        }
    }
}
