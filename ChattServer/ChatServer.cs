using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ChattCommon;

namespace ChattServer
{
    public class ChatServer
    {
        private const int PORT = 5000;
        private TcpListener _listener;
        private readonly List<ClientConnection> _clients = new();
        private readonly Logger _logger;
        private int _clientIdCounter = 1;
        private readonly object _lockObj = new();

        public ChatServer()
        {
            _logger = new Logger("server.log");
        }

        public void Start()
        {
            try
            {
                _listener = new TcpListener(IPAddress.Any, PORT);
                _listener.Start();
                _logger.Log("Server startad");
                Console.WriteLine($"Servern lyssnar på port {PORT}...");

                while (true)
                {
                    var client = _listener.AcceptTcpClient();
                    var thread = new Thread(() => HandleClient(client))
                    {
                        IsBackground = true
                    };
                    thread.Start();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Serverfel", ex);
            }
        }

        private void HandleClient(TcpClient client)
        {
            ClientConnection connection = null;
            try
            {
                lock (_lockObj)
                {
                    connection = new ClientConnection(client, _clientIdCounter++);
                    _clients.Add(connection);
                }

                var user = connection.User;
                _logger.Log($"Ansluten: {user.Username}");
                BroadcastMessage(new Message("System", $"{user.Username} har anslutit."));

                string text;
                while ((text = connection.ReadMessage()) != null)
                {
                    if (string.IsNullOrWhiteSpace(text))
                        continue;

                    var message = new Message(user.Username, text);
                    _logger.Log(message.ToString());
                    BroadcastMessage(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Fel i klienthantering", ex);
            }
            finally
            {
                if (connection != null)
                {
                    lock (_lockObj)
                    {
                        _clients.Remove(connection);
                    }

                    var name = connection.User?.Username ?? "Okänd";
                    _logger.Log($"Frånkopplad: {name}");
                    BroadcastMessage(new Message("System", $"{name} har lämnat chatten."));
                    connection.Close();
                }
            }
        }

        private void BroadcastMessage(Message message)
        {
            lock (_lockObj)
            {
                foreach (var client in _clients.ToArray())
                {
                    try
                    {
                        client.SendMessage(message);
                    }
                    catch
                    {
                        // ignore, klienten tas bort vid nästa felhantering
                    }
                }
            }
        }
    }
}
