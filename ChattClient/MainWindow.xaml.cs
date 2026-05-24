using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using ChattCommon;

namespace ChattClient
{
    /// <summary>
    /// Interaktionslogik för MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ServerConnection _connection;
        private const string Host = "127.0.0.1";
        private const int Port = 5000;
        private const string PlaceholderText = "Skriv ett meddelande...";

        public MainWindow()
        {
            InitializeComponent();
            _connection = new ServerConnection();
            _connection.MessageReceived += OnMessageReceived;
            _connection.ConnectionStatusChanged += OnConnectionStatusChanged;
            MessageInput.IsEnabled = false;
            MessageInput.Foreground = Brushes.Gray;
            MessageInput.Text = PlaceholderText;
            ImageButton.IsEnabled = false;
        }

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (_connection.IsConnected)
            {
                _connection.Disconnect();
                SetDisconnectedState();
                return;
            }

            string username = UsernameInput.Text.Trim();
            if (string.IsNullOrEmpty(username) || username.Length < 3)
            {
                MessageBox.Show("Ange ett användarnamn med minst 3 tecken.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ConnectButton.IsEnabled = false;
            ConnectButton.Content = "Ansluter...";

            bool success = await _connection.ConnectAsync(Host, Port, username);

            if (success)
            {
                ConnectButton.Content = "Koppla från";
                UsernameInput.IsEnabled = false;
                MessageInput.IsEnabled = true;
                ImageButton.IsEnabled = true;
                MessageInput.Focus();
                StatusText.Text = $"✓ Ansluten som {username}";
                StatusText.Foreground = Brushes.LightGreen;
            }
            else
            {
                ConnectButton.Content = "Anslut";
            }

            ConnectButton.IsEnabled = true;
        }

        private void UsernameInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ConnectButton_Click(sender, e);
                e.Handled = true;
            }
        }

        private void MessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendCurrentMessage();
                e.Handled = true;
            }
        }

        private void SendCurrentMessage()
        {
            var message = MessageInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(message) || message == PlaceholderText)
                return;

            if (!_connection.IsConnected)
            {
                MessageBox.Show("Du är inte ansluten till servern!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _connection.SendMessage(message);
            MessageInput.Clear();
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_connection.IsConnected)
            {
                MessageBox.Show("Logga in först innan du skickar en bild.", "Ingen anslutning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new OpenFileDialog
            {
                Title = "Välj en bild att skicka",
                Filter = "Bildfiler (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp"
            };

            if (dialog.ShowDialog() != true)
                return;

            try
            {
                var bytes = File.ReadAllBytes(dialog.FileName);
                var imageData = Convert.ToBase64String(bytes);
                _connection.SendImage(Path.GetFileName(dialog.FileName), imageData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kunde inte läsa bilden: {ex.Message}", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnMessageReceived(Message message)
        {
            Dispatcher.Invoke(() => DisplayMessage(message));
        }

        private void DisplayMessage(Message message)
        {
            var isSelf = _connection.IsConnected && message.Sender == _connection.Username;
            var messageContainer = new System.Windows.Controls.StackPanel
            {
                Margin = new Thickness(0, 0, 0, 16),
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = Double.NaN
            };

            var header = new System.Windows.Controls.TextBlock
            {
                FontSize = 13,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 6)
            };
            header.Inlines.Add(new System.Windows.Documents.Run($"[{message.Timestamp:HH:mm:ss}] ") { Foreground = Brushes.Gray });
            header.Inlines.Add(new System.Windows.Documents.Run(message.Sender) { Foreground = Brushes.LightSkyBlue });
            messageContainer.Children.Add(header);

            var bubble = new System.Windows.Controls.Border
            {
                Background = new SolidColorBrush(isSelf ? Color.FromRgb(63, 86, 127) : Color.FromRgb(40, 44, 52)),
                CornerRadius = new CornerRadius(14),
                Padding = new Thickness(14),
                MaxWidth = 520,
                HorizontalAlignment = HorizontalAlignment.Left,
                MinWidth = 120,
                Width = Double.NaN
            };

            var bubbleStack = new System.Windows.Controls.StackPanel();

            if (!string.IsNullOrWhiteSpace(message.Content))
            {
                var body = new System.Windows.Controls.TextBlock
                {
                    Text = message.Content,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = Brushes.White,
                    Margin = new Thickness(0, 0, 0, message.HasImage ? 10 : 0),
                    MaxWidth = 460
                };
                bubbleStack.Children.Add(body);
            }

            if (message.HasImage)
            {
                try
                {
                    var bitmap = new BitmapImage();
                    var bytes = Convert.FromBase64String(message.ImageData);
                    using var stream = new MemoryStream(bytes);
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.DecodePixelWidth = 360;
                    bitmap.EndInit();
                    bitmap.Freeze();

                    var imageControl = new System.Windows.Controls.Image
                    {
                        Source = bitmap,
                        MaxWidth = 360,
                        MaxHeight = 360,
                        Stretch = Stretch.Uniform,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(0, 0, 0, 0)
                    };
                    bubbleStack.Children.Add(imageControl);
                }
                catch
                {
                    var error = new System.Windows.Controls.TextBlock
                    {
                        Text = "[Bild kunde inte visas]",
                        Foreground = Brushes.Orange,
                        Margin = new Thickness(0, 8, 0, 0)
                    };
                    bubbleStack.Children.Add(error);
                }
            }

            bubble.Child = bubbleStack;
            messageContainer.Children.Add(bubble);
            MessagePanel.Children.Add(messageContainer);
            MessageScrollViewer.ScrollToEnd();
        }

        private void OnConnectionStatusChanged(string status)
        {
            Dispatcher.Invoke(() =>
            {
                StatusText.Text = status;
                if (status.StartsWith("✓"))
                {
                    StatusText.Foreground = Brushes.LightGreen;
                }
                else
                {
                    StatusText.Foreground = Brushes.OrangeRed;
                    SetDisconnectedState();
                }
            });
        }

        private void SetDisconnectedState()
        {
            ConnectButton.Content = "Anslut";
            UsernameInput.IsEnabled = true;
            MessageInput.IsEnabled = false;
            ImageButton.IsEnabled = false;
            MessageInput.Text = PlaceholderText;
            MessageInput.Foreground = Brushes.Gray;
        }

        private void MessageInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MessageInput.Text == PlaceholderText)
            {
                MessageInput.Text = string.Empty;
                MessageInput.Foreground = Brushes.White;
            }
        }

        private void MessageInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MessageInput.Text))
            {
                MessageInput.Text = PlaceholderText;
                MessageInput.Foreground = Brushes.Gray;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_connection.IsConnected)
                _connection.Disconnect();
        }
    }
}
