using Newtonsoft.Json;
using System;

namespace SpeedTest.Net.Models
{
    public class Server
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("host", NullValueHandling = NullValueHandling.Ignore)]
        public string Host { get; set; }

        [JsonProperty("lat", NullValueHandling = NullValueHandling.Ignore)]
        public double Latitude { get; set; }

        [JsonProperty("lon", NullValueHandling = NullValueHandling.Ignore)]
        public double Longitude { get; set; }

        [JsonIgnore]
        internal string Url => $"http://{Host}/SpeedTest.Net/upload.php";

        [JsonIgnore]
        private readonly Lazy<Coordinate> geoCoordinate;

        [JsonIgnore]
        internal Coordinate GeoCoordinate
        {
            get { return geoCoordinate.Value; }
        }

        [JsonIgnore]
        internal double Distance { get; set; }

        public Server()
        {
            // note: geo coordinate will not be recalculated on Latitude or Longitude change
            geoCoordinate = new Lazy<Coordinate>(() => new Coordinate(Latitude, Longitude));
        }
    }
}