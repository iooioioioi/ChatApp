using System;

namespace ChattCommon
{
    /// <summary>
    /// Representerar en användare i chatsystemet.
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
