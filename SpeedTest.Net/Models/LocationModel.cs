using Newtonsoft.Json;
using System.Linq;

namespace SpeedTest.Net.Models
{
    internal partial class LocationModel
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("loc")]
        public string Loc { get; set; }

        [JsonProperty("org")]
        public string Org { get; set; }


        public double Latitude => double.Parse(Loc?.Split(',')?.FirstOrDefault()?.Trim() ?? "0");
        public double Longitude => double.Parse(Loc?.Split(',')?.LastOrDefault()?.Trim() ?? "0");
    }
}