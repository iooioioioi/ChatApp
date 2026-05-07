using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ChattCommon;

namespace ChattClient
{
    public class ServerConnection
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;
        private readonly Logger _logger;
        private bool _isRunning;

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
                _client = new TcpClient();
                var connectTask = _client.ConnectAsync(host, port);
                if (await Task.WhenAny(connectTask, Task.Delay(5000)) != connectTask)
                {
                    throw new TimeoutException("Timeout vid anslutning");
                }

                _stream = _client.GetStream();
                _reader = new StreamReader(_stream, Encoding.UTF8);
                _writer = new StreamWriter(_stream, Encoding.UTF8) { AutoFlush = true };

                _writer.WriteLine(username);
                _isRunning = true;
                IsConnected = true;
                _logger.Log($"Ansluten som {username}");
                ConnectionStatusChanged?.Invoke($"✓ Ansluten som {username}");

                _ = Task.Run(ReceiveMessagesAsync);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Anslutningsfel", ex);
                ConnectionStatusChanged?.Invoke($"✗ Anslutningsfel: {ex.Message}");
                IsConnected = false;
                return false;
            }
        }

        private async Task ReceiveMessagesAsync()
        {
            try
            {
                while (_isRunning && _client?.Connected == true)
                {
                    var line = await _reader.ReadLineAsync();
                    if (line == null)
                        break;

                    var message = Message.Deserialize(line);
                    MessageReceived?.Invoke(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Mottagningsfel", ex);
            }
            finally
            {
                Disconnect();
            }
        }

        public void SendMessage(string content)
        {
            if (!IsConnected || string.IsNullOrWhiteSpace(content))
                return;

            try
            {
                _writer.WriteLine(content);
                _logger.Log($"Skickade: {content}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Fel vid skickning", ex);
            }
        }

        public void Disconnect()
        {
            if (!IsConnected)
                return;

            _isRunning = false;
            IsConnected = false;

            try
            {
                _writer?.Close();
                _reader?.Close();
                _stream?.Close();
                _client?.Close();
            }
            catch
            {
            }

            ConnectionStatusChanged?.Invoke("✗ Frånkopplad");
            _logger.Log("Frånkopplad från server");
        }
    }
}
