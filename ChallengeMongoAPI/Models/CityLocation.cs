using System.Text.Json.Serialization;

namespace ChallengeMongoAPI.Models
{
    public class CityLocation
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("latitude")]
        public float Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public float Longitude { get; set; }
    }

    public class Locations
    {
        [JsonPropertyName("results")]
        public List<CityLocation>? CityLocations { get; set; }
    }
}
