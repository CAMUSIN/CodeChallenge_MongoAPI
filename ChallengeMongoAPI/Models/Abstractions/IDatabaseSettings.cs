namespace ChallengeMongoAPI.Models.Abstractions
{
    public interface IDatabaseSettings
    {
        string DatabaseName { get; set; }

        string CollectionName { get; set; }

        string ConnectionString { get; set; }
    }
}
