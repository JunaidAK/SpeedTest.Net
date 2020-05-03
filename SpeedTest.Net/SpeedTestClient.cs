using System.Threading.Tasks;
using SpeedTest.Net.Enums;
using SpeedTest.Net.Models;

namespace SpeedTest.Net
{
    public static class SpeedTestClient
    {
        private static SpeedTestHttpClient Client => new SpeedTestHttpClient();

        /// <summary>
        /// Calculates download speed using the provided server
        /// </summary>
        /// <param name="server">The server object used for downloading files</param>
        /// <param name="unit">Unit in which Speed Test response should be returned</param>
        /// <returns>An instance of type DownloadSpeed</returns>
        public static async Task<DownloadSpeed> GetDownloadSpeed(Server server = null, SpeedTestUnit unit = SpeedTestUnit.KiloBytesPerSecond) => await Client.GetDownloadSpeed(server, unit);

        /// <summary>
        /// Finds the closest server to the provided co-ordinates
        /// </summary>
        /// <param name="latitude">Latitude of the location</param>
        /// <param name="longitude">Longitude of the location</param>
        /// <returns>An instance of type Server close to the provided latitude and longitude</returns>
        public static async Task<Server> GetServer(double latitude, double longitude) => await Client.GetServer(latitude, longitude);

        /// <summary>
        /// Finds the best server based on the callee location
        /// </summary>
        /// <returns>An instance of type Server close to the callee location</returns>
        public static async Task<Server> GetServer() => await Client.GetServer();
    }
}