using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace ChallengeMongoAPI.Models
{
    public class Forecast
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [JsonPropertyName("latitude")]
        [BsonElement("latitude")]
        public float? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        [BsonElement("longitude")]
        public float? Longitude { get; set; }

        [JsonPropertyName("timezone")]
        [BsonElement("timezone")]
        public string? Timezone { get; set; }

        [JsonPropertyName("elevation")]
        [BsonElement("elevation")]
        public float Elevation { get; set; }

        [JsonPropertyName("current_weather")]
        [BsonElement("current_weather")]
        public Weather? Currentweather { get; set; }

        [JsonPropertyName("daily")]
        [BsonElement("daily_sunrise")]
        public Daily? DayliSunrise { get; set; }

        [BsonElement("city")]
        public string City { get; set; } = String.Empty;

        [JsonPropertyName("generationtime_ms")]
        [BsonElement("time")]
        public double? Time { get; set; }
    }
}
