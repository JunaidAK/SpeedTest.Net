using SpeedTest.Net.Enums;
using SpeedTest.Net.Models;
using System.Threading.Tasks;

namespace SpeedTest.Net
{
    public static class FastClient
    {
        private static FastHttpClient Client => new FastHttpClient();

        /// <summary>
        /// Calculates download speed using the provided server
        /// </summary>
        /// <param name="unit">Specifies in which unit download speed should be returned</param>
        /// <returns>An instance of type DownloadSpeed</returns>
        public static async Task<DownloadSpeed> GetDownloadSpeed(SpeedTestUnit unit = SpeedTestUnit.KiloBytesPerSecond) => await Client?.GetDownloadSpeed(unit);
    }
}