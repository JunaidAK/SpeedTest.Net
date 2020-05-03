using Newtonsoft.Json;
using System;

namespace SpeedTest.Net.Models
{
    public partial class FileUrl
    {
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }
    }
}