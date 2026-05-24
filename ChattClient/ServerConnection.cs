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
    /// Hanterar anslutningen och kommunikationen med servern.
    /// </summary>
    public class ServerConnection
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;

        private readonly Logger _logger;
        private string _username;

        // Event som aktiveras när ett meddelande mottas
        public event Action<Message> MessageReceived;
        public event Action<string> ConnectionStatusChanged;

        private bool _isRunning = false;

        public ServerConnection()
        {
            _logger = new Logger("client.log");
        }

        public string Username => _username;

        /// <summary>
        /// Ansluter till servern.
        /// </summary>
        public async Task<bool> ConnectAsync(string host, int port, string username)
        {
            try
            {
                _username = username;
                _client = new TcpClient();

                // Anslutning med timeout
                var connectTask = _client.ConnectAsync(host, port);
                if (await Task.WhenAny(connectTask, Task.Delay(5000)) != connectTask)
                {
                    throw new TimeoutException("Anslutningen tog för lång tid.");
                }

                _stream = _client.GetStream();
                _reader = new StreamReader(_stream, Encoding.UTF8);
                _writer = new StreamWriter(_stream, Encoding.UTF8) { AutoFlush = true };

                // Skicka användarnamn till servern
                _writer.WriteLine(username);

                _isRunning = true;
                _logger.Log($"Ansluten till {host}:{port} som '{username}'");
                ConnectionStatusChanged?.Invoke($"✓ Ansluten till servern som {username}");

                // Starta tråd för att tas emot meddelanden
                var receiveTask = ReceiveMessagesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Anslutningsfel", ex);
                ConnectionStatusChanged?.Invoke($"✗ Anslutningsfel: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Läser meddelanden från servern kontinuerligt.
        /// </summary>
        private async Task ReceiveMessagesAsync()
        {
            try
            {
                while (_isRunning && _client?.Connected == true)
                {
                    string line = await _reader.ReadLineAsync();

                    if (line == null)
                        break;

                    var message = Message.Deserialize(line);
                    MessageReceived?.Invoke(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Fel vid mottagning av meddelande", ex);
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Skickar ett textmeddelande till servern.
        /// </summary>
        public void SendMessage(string content)
        {
            try
            {
                if (_writer != null && _client?.Connected == true)
                {
                    var message = new Message(_username, content);
                    _writer.WriteLine(message.Serialize());
                    _logger.Log($"Meddelande skickat: {content}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Fel vid skickning", ex);
            }
        }

        /// <summary>
        /// Skickar en bild till servern.
        /// </summary>
        public void SendImage(string imageName, string imageData)
        {
            try
            {
                if (_writer != null && _client?.Connected == true)
                {
                    var message = new Message(_username, $"[Bild: {imageName}]", imageName, imageData);
                    _writer.WriteLine(message.Serialize());
                    _logger.Log($"Bild skickad: {imageName}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Fel vid bildskick", ex);
            }
        }

        /// <summary>
        /// Kopplas från servern.
        /// </summary>
        public void Disconnect()
        {
            _isRunning = false;

            try
            {
                _writer?.Close();
                _reader?.Close();
                _stream?.Close();
                _client?.Close();
            }
            catch { }

            ConnectionStatusChanged?.Invoke("✗ Frånkopplad från servern");
            _logger.Log("Frånkopplad från servern");
        }

        public bool IsConnected => _client?.Connected == true;
    }
}
