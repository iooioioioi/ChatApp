using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
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
            SetLoginVisible();
        }

        private void SetLoginVisible()
        {
            LoginGrid.Visibility = Visibility.Visible;
            ChatGrid.Visibility = Visibility.Collapsed;
            SetStatusIndicator(Brushes.Gray);
            MessageInput.Foreground = Brushes.Gray;
            MessageInput.Text = "Skriv ett meddelande...";
            LoginButton.IsEnabled = true;
        }

        private void SetChatVisible()
        {
            LoginGrid.Visibility = Visibility.Collapsed;
            ChatGrid.Visibility = Visibility.Visible;
            MessageInput.IsEnabled = true;
            MessageInput.Focus();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            await LoginAsync();
        }

        private async void LoginUsernameInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await LoginAsync();
                e.Handled = true;
            }
        }

        private async Task<bool> LoginAsync()
        {
            var username = UsernameInput.Text.Trim();
            if (!ValidateUsername(username))
            {
                MessageBox.Show("Användarnamnet måste vara minst 3 tecken och får bara innehålla bokstäver, siffror, '_' eller '-'.", "Ogiltigt användarnamn", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            LoginButton.IsEnabled = false;
            SetStatusIndicator(Brushes.Goldenrod);

            var success = await _connection.ConnectAsync("127.0.0.1", 5000, username);
            if (success)
            {
                SetStatusIndicator(Brushes.Green);
                SetChatVisible();
            }
            else
            {
                SetStatusIndicator(Brushes.Gray);
                LoginButton.IsEnabled = true;
            }

            return success;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            _connection.Disconnect();
            UsernameInput.Clear();
            SetLoginVisible();
        }

        private void SendCurrentMessage()
        {
            var message = MessageInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(message) || !_connection.IsConnected || message == "Skriv ett meddelande...")
                return;

            _connection.SendMessage(message);
            MessageInput.Clear();
            MessageInput.Focus();
        }

        private void MessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendCurrentMessage();
                e.Handled = true;
            }
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
                Title = "Välj bild att skicka",
                Filter = "Bildfiler (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp"
            };

            if (dialog.ShowDialog() != true)
                return;

            try
            {
                var imageBytes = File.ReadAllBytes(dialog.FileName);
                var imageData = Convert.ToBase64String(imageBytes);
                _connection.SendImage(Path.GetFileName(dialog.FileName), imageData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kunde inte läsa bilden: {ex.Message}", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnMessageReceived(Message message)
        {
            DisplayMessage(message);
        }

        private void DisplayMessage(Message message)
        {
            Dispatcher.Invoke(() =>
            {
                var messageContainer = new System.Windows.Controls.StackPanel
                {
                    Margin = new Thickness(0, 0, 0, 16)
                };

                var header = new System.Windows.Controls.TextBlock
                {
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 13,
                    Margin = new Thickness(0, 0, 0, 8)
                };
                header.Inlines.Add(new System.Windows.Documents.Run($"[{message.Timestamp:HH:mm:ss}] ") { Foreground = Brushes.Gray });
                header.Inlines.Add(new System.Windows.Documents.Run(message.Sender) { Foreground = Brushes.LightSkyBlue });
                messageContainer.Children.Add(header);

                var bubble = new System.Windows.Controls.Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(54, 57, 63)),
                    CornerRadius = new CornerRadius(14),
                    Padding = new Thickness(14),
                    //MaxWidth = 880
                };

                var bubbleStack = new System.Windows.Controls.StackPanel();

                if (!string.IsNullOrWhiteSpace(message.Content))
                {
                    var body = new System.Windows.Controls.TextBlock
                    {
                        Text = message.Content,
                        TextWrapping = TextWrapping.Wrap,
                        Foreground = Brushes.White,
                        Margin = new Thickness(0, 0, 0, message.HasImage ? 10 : 0)
                    };
                    bubbleStack.Children.Add(body);
                }

                if (message.HasImage)
                {
                    try
                    {
                        var image = new BitmapImage();
                        var bytes = Convert.FromBase64String(message.ImageData);
                        using var stream = new MemoryStream(bytes);
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = stream;
                        image.DecodePixelWidth = 420;
                        image.EndInit();
                        image.Freeze();

                        var imageControl = new System.Windows.Controls.Image
                        {
                            Source = image,
                            MaxWidth = 420,
                            MaxHeight = 420,
                            Margin = new Thickness(0, 0, 0, 0),
                            Stretch = Stretch.Uniform,
                            HorizontalAlignment = HorizontalAlignment.Right
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
            });
        }

        private void OnConnectionStatusChanged(string status)
        {
            Dispatcher.Invoke(() =>
            {
                if (status.StartsWith("✓"))
                    SetStatusIndicator(Brushes.Green);
                else if (status.StartsWith("⏳"))
                    SetStatusIndicator(Brushes.Goldenrod);
                else
                    SetStatusIndicator(Brushes.Gray);
            });
        }

        private void SetStatusIndicator(Brush brush)
        {
            StatusIndicator.Fill = brush;
        }

        private bool ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            if (username.Length < 3)
                return false;

            foreach (var c in username)
            {
                if (!char.IsLetterOrDigit(c) && c != '_' && c != '-')
                    return false;
            }

            return true;
        }

        private void MessageInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MessageInput.Text == "Skriv ett meddelande...")
            {
                MessageInput.Clear();
                MessageInput.Foreground = Brushes.White;
            }
        }

        private void MessageInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MessageInput.Text))
            {
                MessageInput.Text = "Skriv ett meddelande...";
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
