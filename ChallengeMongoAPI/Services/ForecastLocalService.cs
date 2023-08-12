using ChallengeMongoAPI.Models;
using ChallengeMongoAPI.Models.Abstractions;
using ChallengeMongoAPI.Services.Abstractions;
using MongoDB.Driver;

namespace ChallengeMongoAPI.Services
{
    public class ForecastLocalService : IForecastLocalService
    {
        private readonly IMongoCollection<Forecast> _forecastCollection;
        public ForecastLocalService(IDatabaseSettings settings, IMongoClient mongoClient)
        {
            _forecastCollection = mongoClient
                .GetDatabase(settings.DatabaseName)
                .GetCollection<Forecast>(settings.CollectionName);
        }

        public async Task<Forecast> CreateAsync(Forecast forecast)
        {
            await _forecastCollection.InsertOneAsync(forecast);
            return forecast;
        }

        public Forecast Get(float lat, float lon)
        {
            return _forecastCollection.Find(forecast =>
                forecast.Latitude == lat 
                && forecast.Longitude == lon 
                && forecast.DayliSunrise.Time[0].Equals(DateTime.Now.ToString("yyyy-MM-dd"))
                ).FirstOrDefault();
        }

        public Forecast GetByCity(string city)
        {
            return _forecastCollection.Find(x => 
                x.City == city.ToUpper() 
                && x.DayliSunrise.Time[0].Equals(DateTime.Now.ToString("yyyy-MM-dd"))
                ).FirstOrDefault();
        }
    }
}
