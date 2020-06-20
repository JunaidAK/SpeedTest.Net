using System.Linq;
using System.Text.Json.Serialization;

namespace SpeedTest.Net.Models
{
    internal partial class LocationModel
    {
        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("loc")]
        public string Loc { get; set; }

        [JsonPropertyName("org")]
        public string Org { get; set; }


        public double Latitude => double.Parse(Loc?.Split(',')?.FirstOrDefault()?.Trim() ?? "0");
        public double Longitude => double.Parse(Loc?.Split(',')?.LastOrDefault()?.Trim() ?? "0");
    }
}
