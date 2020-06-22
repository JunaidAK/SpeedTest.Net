using System;
using System.Text.Json.Serialization;

namespace SpeedTest.Net.Models
{
    public partial class FileUrl
    {
        [JsonPropertyName("url")]
        public Uri Url { get; set; }
    }
}
