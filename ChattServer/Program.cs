using System;
using ChattCommon;

namespace ChattServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════╗");
            Console.WriteLine("║   CHATT-SERVER STARTAS...      ║");
            Console.WriteLine("╚════════════════════════════════╝\n");

            ChatServer server = new ChatServer();
            server.Start();
        }
    }
}
