using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace ChallengeMongoAPI.Models
{
    public class Weather
    {
        [JsonPropertyName("temperature")]
        [BsonElement("temperature")]
        public float Temperature { get; set; }

        [JsonPropertyName("windspeed")]
        [BsonElement("wind_speed")]
        public float WindSpeed { get; set; }

        [JsonPropertyName("winddirection")]
        [BsonElement("wind_direction")]
        public float WindDirection { get; set; }

        [JsonPropertyName("weathercode")]
        [BsonElement("weather_code")]
        public int WeatherCode { get; set; }

        [JsonPropertyName("is_day")]
        [BsonElement("is_day")]
        public int IsDay { get; set; }

        [JsonPropertyName("time")]
        [BsonElement("time")]
        public string? Time { get; set; }
    }
}
