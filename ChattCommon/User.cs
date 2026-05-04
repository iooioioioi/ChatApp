using System;

namespace ChattCommon
{
    /// <summary>
    /// TODO: Implementera User-klassen
    /// Ska representera en användare i chatsystemet.
    /// Krav:
    /// - Properties: Username (string), Id (int), ConnectedTime (DateTime)
    /// - Konstruktor: User(string username, int id)
    /// - Metod: ToString() - formaterad visning
    /// </summary>
    public class User
    {
        public string Username { get; set; }
        public int Id { get; set; }
        public DateTime ConnectedTime { get; set; }

        public User(string username, int id)
        {
            Username = username ?? "Anonymous";
            Id = id;
            ConnectedTime = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Username} (ID: {Id})";
        }
    }
}
