using System;
using System.Text.Json.Serialization;

namespace SpeedTest.Net.Models
{
    public class Server
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("host")]
        public string Host { get; set; }

        [JsonPropertyName("lat")]
        public double Latitude { get; set; }

        [JsonPropertyName("lon")]
        public double Longitude { get; set; }

        [JsonIgnore]
        internal string Url => $"http://{Host}/SpeedTest.Net/upload.php";
        
        private readonly Lazy<Coordinate> _geoCoordinate;

        [JsonIgnore]
        internal Coordinate GeoCoordinate => _geoCoordinate.Value;

        [JsonIgnore]
        internal double Distance { get; set; }

        public Server()
        {
            // note: geo coordinate will not be recalculated on Latitude or Longitude change
            _geoCoordinate = new Lazy<Coordinate>(() => new Coordinate(Latitude, Longitude));
        }
    }
}
