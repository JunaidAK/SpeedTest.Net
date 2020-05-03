using SpeedTest.Net.Enums;
using SpeedTest.Net.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace SpeedTest.Net.Usage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();

            Messages.CollectionChanged += Messages_CollectionChanged;
        }

        private void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyOfPropertyChanged("Messages");
        }

        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();


        private Server Server { get; set; }

        private void GetConnectionSpeed(object sender, RoutedEventArgs e)
        {
            if (Server == null)
            {
                MessageBox.Show("Fetch Server first");
                return;
            }

            ExecuteSpeedTest(SpeedTestSource.Speedtest, Server);
        }

        private void GetConnectionSpeedUsingFast(object sender, RoutedEventArgs e)
        {
            ExecuteSpeedTest(SpeedTestSource.Fast);
        }

        private void GetConnectionSpeedLocal(object sender, RoutedEventArgs e)
        {
            ExecuteSpeedTest(SpeedTestSource.Speedtest);
        }

        private void CopyToClipboard(object sender, RoutedEventArgs e)
        {
            CopyToClipboard((sender as System.Windows.Controls.MenuItem).DataContext.ToString());
        }

        private async void FetchServer(object sender, RoutedEventArgs e)
        {
            try
            {
                SpeedGrid.IsEnabled = false;
                Server = await SpeedTestClient.GetServer();
                ShowMessage($"Server Fetched: {Server.Host} ({Server.Id})");
                SpeedGrid.IsEnabled = true;
            }
            catch (System.Exception ex)
            {
                ShowMessage(ex.Message);
                SpeedGrid.IsEnabled = true;
            }
        }

        private async void ExecuteSpeedTest(SpeedTestSource source, Server server = null)
        {
            try
            {
                SpeedGrid.IsEnabled = false;

                DownloadSpeed speed = null;

                if (source == SpeedTestSource.Speedtest)
                    speed = await SpeedTestClient.GetDownloadSpeed(server, SpeedTestUnit.KiloBitsPerSecond);
                else
                    speed = await FastClient.GetDownloadSpeed(SpeedTestUnit.KiloBitsPerSecond);

                var message = $"Source: {speed.Source} Download Speed: {speed?.Speed} {speed.Unit}";
                
                if (speed?.Server?.Id != null)
                    message += $" (Server Id = {speed?.Server?.Id})";

                ShowMessage(message);
                SpeedGrid.IsEnabled = true;
            }
            catch (System.Exception ex)
            {
                ShowMessage(ex.Message);
                SpeedGrid.IsEnabled = true; 
            }
        }

        private void ShowMessage(string message)
        {
            Messages.Add(message);
        }

        private void CopyToClipboard(string message)
        {
            Clipboard.SetText(message);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyOfPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}