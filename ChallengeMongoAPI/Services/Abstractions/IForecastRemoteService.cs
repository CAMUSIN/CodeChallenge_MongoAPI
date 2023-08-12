using ChallengeMongoAPI.Models;

namespace ChallengeMongoAPI.Services.Abstractions
{
    public interface IForecastRemoteService
    {
        Task<Forecast> Get(float lat, float lon);

        Task<CityLocation> GetCityLocation(string city);
    }
}
