using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using ChattCommon;

namespace ChattServer
{
    /// <summary>
    /// TODO: Implementera ChatServer-klassen
    /// Huvudservern för chatsystemet. Hanterar klientanslutningar och distribuerar meddelanden.
    /// Krav:
    /// - Privat const int PORT = 5000
    /// - Privat TcpListener _listener
    /// - Privat List<ClientConnection> _clients
    /// - Privat Logger _logger
    /// - Privat object _lockObj för thread-safety
    /// - Metod: Start() - startar servern och accepterar anslutningar
    /// - Metod: HandleClient(TcpClient) - hanterar individuell klientanslutning (i tråd)
    /// - Metod: BroadcastMessage(Message) - skickar meddelande till alla klienter
    /// </summary>
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
                _logger.Log("===== SERVER STARTAD =====");
                Console.WriteLine($"Servern lyssnar på port {PORT}...");

                // TODO: Acceptera klientanslutningar i loop
                while (true)
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    // TODO: Skapa tråd för HandleClient
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Serverfel", ex);
            }
        }

        private void HandleClient(TcpClient client)
        {
            // TODO: Implementera
            throw new NotImplementedException();
        }

        private void BroadcastMessage(Message message)
        {
            // TODO: Implementera
            throw new NotImplementedException();
        }
    }
}
