using System;

namespace ChattCommon
{
    public class User
    {
        public string Username { get; set; }
        public int Id { get; set; }
        public DateTime ConnectedTime { get; set; }

        public User(string username, int id)
        {
            Username = string.IsNullOrWhiteSpace(username) ? "Anonymous" : username;
            Id = id;
            ConnectedTime = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Username} (ID: {Id})";
        }
    }
}
