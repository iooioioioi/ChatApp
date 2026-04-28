using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ChattCommon;

namespace ChattClient
{
    /// <summary>
    /// Interaktionslogik för MainWindow.xaml
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

        /// <summary>
        /// Anslutningsknapp klickad
        /// </summary>
        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (_connection.IsConnected)
            {
                _connection.Disconnect();
                ConnectButton.Content = "Anslut";
                MessageInput.IsEnabled = false;
                SendButton.IsEnabled = false;
                return;
            }

            string host = ServerInput.Text.Trim();
            string portStr = PortInput.Text.Trim();
            string username = UsernameInput.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Ange ett användarnamn!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(portStr, out int port))
            {
                MessageBox.Show("Ogiltigt portnummer!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ConnectButton.IsEnabled = false;
            ConnectButton.Content = "Ansluter...";

            bool success = await _connection.ConnectAsync(host, port, username);

            if (success)
            {
                ConnectButton.Content = "Koppla från";
                MessageInput.IsEnabled = true;
                SendButton.IsEnabled = true;
                ServerInput.IsEnabled = false;
                PortInput.IsEnabled = false;
                UsernameInput.IsEnabled = false;
            }
            else
            {
                ConnectButton.Content = "Anslut";
            }

            ConnectButton.IsEnabled = true;
        }

        /// <summary>
        /// Skicka-knapp klickad
        /// </summary>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageInput.Text.Trim();

            if (string.IsNullOrEmpty(message))
                return;

            if (!_connection.IsConnected)
            {
                MessageBox.Show("Du är inte ansluten till servern!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _connection.SendMessage(message);
            MessageInput.Clear();
            MessageInput.Focus();
        }

        /// <summary>
        /// Meddelande mottaget från servern
        /// </summary>
        private void OnMessageReceived(Message message)
        {
            Dispatcher.Invoke(() =>
            {
                string displayText = $"{message.ToString()}\n";
                MessageHistory.Text += displayText;

                // Scrolla ner till sista meddelandet
                if (MessageHistory.Parent is ScrollViewer scroller)
                {
                    scroller.ScrollToEnd();
                }
            });
        }

        /// <summary>
        /// Anslutningsstatus uppdaterad
        /// </summary>
        private void OnConnectionStatusChanged(string status)
        {
            Dispatcher.Invoke(() =>
            {
                StatusText.Text = status;

                if (status.Contains("✓"))
                {
                    StatusText.Foreground = System.Windows.Media.Brushes.Green;
                }
                else
                {
                    StatusText.Foreground = System.Windows.Media.Brushes.Red;
                    ConnectButton.Content = "Anslut";
                    MessageInput.IsEnabled = false;
                    SendButton.IsEnabled = false;
                    ServerInput.IsEnabled = true;
                    PortInput.IsEnabled = true;
                    UsernameInput.IsEnabled = true;
                }
            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_connection.IsConnected)
                _connection.Disconnect();
        }
    }
}
