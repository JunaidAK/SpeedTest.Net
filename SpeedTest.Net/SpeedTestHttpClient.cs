using SpeedTest.Net.Enums;
using SpeedTest.Net.Helpers;
using SpeedTest.Net.LocalData;
using SpeedTest.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpeedTest.Net
{
    internal class SpeedTestHttpClient : BaseHttpClient
    {
        private readonly ServersList ServersConfig;

        private readonly int[] DownloadSizes = { 350, 750, 1500, 3000 };

        private IEnumerable<string> GenerateDownloadUrls(Server server, int retryCount = 1)
        {
            var downloadUriBase = new Uri(new Uri(server.Url), ".").OriginalString + "random{0}x{0}.jpg?r={1}";

            foreach (var downloadSize in DownloadSizes)
            {
                for (var i = 0; i < retryCount; i++)
                {
                    yield return string.Format(downloadUriBase, downloadSize, i + 1);
                }
            }
        }

        internal async Task<Server> GetServer(double latitude, double longitude)
        {
            try
            {
                return await Task.Factory.StartNew(() =>
                {
                    var _server = new Server() { Latitude = latitude, Longitude = longitude };

                    ServersConfig.CalculateDistances(_server.GeoCoordinate);

                    return ServersConfig.Servers
                        .Where(s => !ServersConfig.IgnoreIds.Contains(s.Id))
                        .OrderBy(s => s.Distance)
                        .FirstOrDefault();
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get Server based on the co-ordinates {latitude},{longitude}", ex);
            }
        }

        internal async Task<Server> GetServer(string ip = "")
        {
            try
            {
                var url = "https://ipinfo.io/json";

                if (!string.IsNullOrEmpty(ip?.Trim()))
                    url = $"https://ipinfo.io/{ip}/json";

                var loc = JsonSerializer.Deserialize<LocationModel>(await GetStringAsync("https://ipinfo.io/json"));
                return await GetServer(loc.Latitude, loc.Longitude);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get Server based on the callee location", ex);
            }
        }

        internal async Task<DownloadSpeed> GetDownloadSpeed(Server server = null, SpeedTestUnit unit = SpeedTestUnit.KiloBytesPerSecond)
        {
            try
            {
                if (server == null)
                    server = await GetServer();

                if (string.IsNullOrEmpty(server?.Url?.Trim()))
                    throw new Exception("Failed to get download speed");

                var downloadUrls = GenerateDownloadUrls(server, 3);

                if (downloadUrls?.Any() != true)
                    throw new Exception("Couldn't fetch downloadable urls");

                var speed = await GetDownloadSpeed(downloadUrls, unit);

                return new DownloadSpeed
                {
                    Server = server,
                    Speed = speed.Speed,
                    Unit = speed.Unit,
                    Source = SpeedTestSource.Speedtest.ToSourceString()
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get download speed", ex);
            }
        }

        public SpeedTestHttpClient() : base(new HttpClientHandler() { Proxy = new WebProxy() })
        {
            Timeout = TimeSpan.FromSeconds(30);

            ServersConfig = new ServersList(
                JsonSerializer.Deserialize<List<Server>>(
                    LocalDataHelper.ReadLocalFile("servers.json")
                )
            );
        }
    }
}