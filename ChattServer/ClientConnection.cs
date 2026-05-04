using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using ChattCommon;

namespace ChattServer
{
    /// <summary>
    /// TODO: Implementera ClientConnection-klassen
    /// Representerar en enskild klientanslutning på servern.
    /// Krav:
    /// - Privat TcpClient _tcpClient
    /// - Privat NetworkStream _stream
    /// - Privat StreamReader _reader
    /// - Privat StreamWriter _writer
    /// - Public User property
    /// - Konstruktor: ClientConnection(TcpClient tcpClient, int clientId)
    ///   - Läser användarnamn från klient (första raden)
    /// - Metod: ReadMessage() - returnerar null om frånkopplad
    /// - Metod: SendMessage(Message) - skickar meddelande till klient
    /// - Metod: Close() - stänger anslutningen
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

            _reader = new StreamReader(_stream, Encoding.UTF8);
            _writer = new StreamWriter(_stream, Encoding.UTF8)
            {
                AutoFlush = true
            };

            // Läs användarnamn från klienten
            string username = _reader.ReadLine() ?? $"User{clientId}";
            User = new User(username, clientId);
        }

        public string ReadMessage()
        {
            // TODO: Implementera läsning från stream
            throw new NotImplementedException();
        }

        public void SendMessage(Message message)
        {
            // TODO: Implementera skickning till stream
            throw new NotImplementedException();
        }

        public void Close()
        {
            // TODO: Implementera stängning av resurser
            throw new NotImplementedException();
        }
    }
}
