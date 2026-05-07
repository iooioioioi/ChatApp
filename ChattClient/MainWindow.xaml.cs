using System;
using System.Windows;
using System.Windows.Media;
using ChattCommon;

namespace ChattClient
{
    public partial class MainWindow : Window
    {
        private readonly ServerConnection _connection;

        public MainWindow()
        {
            InitializeComponent();
            _connection = new ServerConnection();
            _connection.MessageReceived += OnMessageReceived;
            _connection.ConnectionStatusChanged += OnConnectionStatusChanged;
            SetConnected(false);
        }

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (_connection.IsConnected)
            {
                _connection.Disconnect();
                SetConnected(false);
                return;
            }

            var host = ServerInput.Text.Trim();
            var portText = PortInput.Text.Trim();
            var username = UsernameInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(portText) || string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Fyll i server, port och användarnamn.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(portText, out var port))
            {
                MessageBox.Show("Port måste vara ett tal.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ConnectButton.IsEnabled = false;
            ConnectButton.Content = "Ansluter...";

            var success = await _connection.ConnectAsync(host, port, username);
            SetConnected(success);
            ConnectButton.IsEnabled = true;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            var message = MessageInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(message) || !_connection.IsConnected)
                return;

            _connection.SendMessage(message);
            MessageInput.Clear();
            MessageInput.Focus();
        }

        private void OnMessageReceived(Message message)
        {
            Dispatcher.Invoke(() =>
            {
                MessageHistory.Text += message + Environment.NewLine;
                MessageScrollViewer.ScrollToEnd();
            });
        }

        private void OnConnectionStatusChanged(string status)
        {
            Dispatcher.Invoke(() =>
            {
                StatusText.Text = status;
                StatusText.Foreground = status.StartsWith("✓") ? Brushes.Green : Brushes.Red;
            });
        }

        private void SetConnected(bool isConnected)
        {
            SendButton.IsEnabled = isConnected;
            MessageInput.IsEnabled = isConnected;
            ServerInput.IsEnabled = !isConnected;
            PortInput.IsEnabled = !isConnected;
            UsernameInput.IsEnabled = !isConnected;
            ConnectButton.Content = isConnected ? "Koppla från" : "Anslut";
            if (!isConnected)
                StatusText.Text = "✗ Frånkopplad";
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_connection.IsConnected)
                _connection.Disconnect();
        }
    }
}
