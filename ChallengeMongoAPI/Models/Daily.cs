using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace ChallengeMongoAPI.Models
{
    public class Daily
    {
        [JsonPropertyName("time")]
        [BsonElement("time")]
        public string[]? Time { get; set; }

        [JsonPropertyName("sunrise")]
        [BsonElement("sunrise")]
        public string[]? Sunrise { get; set; }
    }
}
