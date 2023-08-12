using ChallengeMongoAPI.Models.Abstractions;

namespace ChallengeMongoAPI.Models
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string DatabaseName { get; set; } = String.Empty;
        public string CollectionName { get; set; } = String.Empty;
        public string ConnectionString { get; set; } = String.Empty;
    }
}
