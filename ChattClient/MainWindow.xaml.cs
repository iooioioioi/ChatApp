using System;
using System.Windows;
using ChattCommon;

namespace ChattClient
{
    /// <summary>
    /// TODO: Implementera MainWindow code-behind
    /// WPF-fönstret för chatten.
    /// Krav:
    /// - Privat ServerConnection _connection
    /// - Event handlers:
    ///   - ConnectButton_Click()
    ///   - SendButton_Click()
    ///   - OnMessageReceived(Message message)
    ///   - OnConnectionStatusChanged(string status)
    /// - Metod: Window_Closed(object sender, EventArgs e)
    /// </summary>
    public partial class MainWindow : Window
    {
        private ServerConnection _connection;

        public MainWindow()
        {
            InitializeComponent();
            _connection = new ServerConnection();
            _connection.MessageReceived += OnMessageReceived;
            _connection.ConnectionStatusChanged += OnConnectionStatusChanged;
        }

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (_connection.IsConnected)
            {
                // TODO: Implementera frånkoppling
                return;
            }

            string username = UsernameInput.Text.Trim();
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Ange ett användarnamn!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // TODO: Hämta host och port, anslut
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageInput.Text.Trim();
            if (string.IsNullOrEmpty(message) || !_connection.IsConnected)
                return;

            // TODO: Implementera skickning
        }

        private void OnMessageReceived(Message message)
        {
            // TODO: Implementera UI-uppdatering
            throw new NotImplementedException();
        }

        private void OnConnectionStatusChanged(string status)
        {
            // TODO: Implementera status-uppdatering
            throw new NotImplementedException();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // TODO: Implementera
            throw new NotImplementedException();
        }
    }
}
