namespace SpeedTest.Net.Models
{
    public class DownloadSpeed
    {
        public Server Server { get; internal set; }
        public double Speed { get; internal set; }
        public string Unit { get; internal set; }
        public string Source { get; set; }
    }
}