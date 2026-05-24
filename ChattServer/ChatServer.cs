using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChattCommon;

namespace ChattServer
{
    /// <summary>
    /// Huvudklassen för chattservern.
    /// Hanterar klientanslutningar och distribuerar meddelanden.
    /// </summary>
    public class ChatServer
    {
        private TcpListener _listener;
        private readonly List<ClientConnection> _clients = new();
        private readonly Logger _logger;
        private int _clientIdCounter = 1;
        private readonly object _lockObj = new();

        private const int PORT = 5000;

        public ChatServer()
        {
            _logger = new Logger("server.log");
        }

        /// <summary>
        /// Startar servern och börjar lyssna på inkommande anslutningar.
        /// </summary>
        public void Start()
        {
            try
            {
                _listener = new TcpListener(IPAddress.Any, PORT);
                _listener.Start();

                _logger.Log("===== SERVER STARTAD =====");
                Console.WriteLine($"Servern lyssnar på port {PORT}...");

                // Acceptera klientanslutningar i en loop
                while (true)
                {
                    TcpClient client = _listener.AcceptTcpClient();

                    // Hantera varje klient i en separat tråd
                    Thread clientThread = new Thread(() => HandleClient(client))
                    {
                        IsBackground = true
                    };
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Serverfel", ex);
                Console.WriteLine($"Serverfel: {ex.Message}");
            }
        }

        /// <summary>
        /// Hanterar en enskild klient-anslutning.
        /// Körs i en separat tråd för varje klient.
        /// </summary>
        private void HandleClient(TcpClient client)
        {
            ClientConnection clientConn = null;

            try
            {
                // Skapa en ny klient-anslutning
                lock (_lockObj)
                {
                    clientConn = new ClientConnection(client, _clientIdCounter++);
                    _clients.Add(clientConn);
                }

                var user = clientConn.User;
                _logger.Log($"Ny anslutning: {user.Username} (ID: {user.Id})");
                Console.WriteLine($"Klient ansluten: {user.Username}");

                // Meddela alla andra klienter
                BroadcastMessage(new Message("System", $"{user.Username} har anslutit sig."));

                // Läs meddelanden från klienten
                string message;
                while ((message = clientConn.ReadMessage()) != null)
                {
                    if (string.IsNullOrWhiteSpace(message))
                        continue;

                    var incoming = Message.Deserialize(message);
                    var chatMessage = new Message(user.Username, incoming.Content, incoming.ImageName, incoming.ImageData)
                    {
                        Timestamp = incoming.Timestamp
                    };
                    _logger.Log(chatMessage.ToString());

                    // Skicka till alla klienter
                    BroadcastMessage(chatMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fel vid hantering av klient", ex);
            }
            finally
            {
                // Ta bort klienten från listan
                if (clientConn != null)
                {
                    lock (_lockObj)
                    {
                        _clients.Remove(clientConn);
                    }

                    _logger.Log($"Klient frånkopplad: {clientConn.User.Username}");
                    Console.WriteLine($"Klient frånkopplad: {clientConn.User.Username}");

                    // Meddela andra klienter
                    BroadcastMessage(new Message("System",
                        $"{clientConn.User.Username} har koppla från."));

                    clientConn.Close();
                }
            }
        }

        /// <summary>
        /// Skickar ett meddelande till alla anslutna klienter.
        /// </summary>
        private void BroadcastMessage(Message message)
        {
            lock (_lockObj)
            {
                foreach (var client in _clients.ToList())
                {
                    try
                    {
                        client.SendMessage(message);
                    }
                    catch
                    {
                        // Klienten är frånkopplad, ta bort senare
                    }
                }
            }
        }

        /// <summary>
        /// Visar statistik om servern.
        /// </summary>
        public void PrintStatus()
        {
            lock (_lockObj)
            {
                Console.WriteLine($"\n=== SERVER STATUS ===");
                Console.WriteLine($"Anslutna klienter: {_clients.Count}");
                foreach (var client in _clients)
                {
                    Console.WriteLine($"  - {client.User.Username}");
                }
            }
        }
    }
}
