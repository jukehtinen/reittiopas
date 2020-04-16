using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Reitti.Web.Models
{
    public class Road
    {
        [JsonPropertyName("mista")]
        public string From { get; set; }
        [JsonPropertyName("mihin")]
        public string To { get; set; }
        [JsonPropertyName("kesto")]
        public int Time { get; set; }
    }

    public class Reittiopas
    {
        [JsonPropertyName("pysakit")]
        public List<string> Stops { get; set; }
        [JsonPropertyName("tiet")]
        public List<Road> Roads { get; set; }
        [JsonPropertyName("linjastot")]
        public Dictionary<string, string[]> Lines { get; set; }
    }
}