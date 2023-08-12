using ChallengeMongoAPI.Models;

namespace ChallengeMongoAPI.Services.Abstractions
{
    public interface IForecastLocalService
    {
        Forecast Get(float lat, float lon);

        Forecast GetByCity(string city);

        Task<Forecast> CreateAsync(Forecast forecast);
    }
}
