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

        // startar servern och väntar på klienter
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

        // hanterar allt som händer för en enskild ansluten klient
        // hanterar en ansluten klient i sin egen tråd
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

                    var message = Message.Deserialize(text);
                    message.Sender = user.Username;
                    message.Timestamp = DateTime.Now;
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

        // skickar meddelandet till alla anslutna klienter
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
                        client.Close();
                        _clients.Remove(client);
                    }
                }
            }
        }
    }
}
