using ChallengeMongoAPI.Models;
using ChallengeMongoAPI.Services.Abstractions;

namespace ChallengeMongoAPI.Services
{
    public class ForecastRemoteService : IForecastRemoteService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ForecastRemoteService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Forecast> Get(float lat, float lon)
        {
            var clientForecast = _httpClientFactory.CreateClient("forecastClient");
            HttpResponseMessage result = await clientForecast.GetAsync($"forecast?latitude={lat}&longitude={lon}&daily=sunrise&current_weather=true&timezone=auto&forecast_days=1");
            if (!result.IsSuccessStatusCode)
            {
                string msg = await result.Content.ReadAsStringAsync();
                throw new Exception(msg);

            }
            Forecast? forecast = await result.Content.ReadFromJsonAsync<Forecast>();
            return forecast;
        }

        public async Task<CityLocation> GetCityLocation(string city)
        {
            var clientGeoCoding = _httpClientFactory.CreateClient("geoCodingClient");
            HttpResponseMessage result = await clientGeoCoding.GetAsync($"search?count=1&language=en&format=json&name={city}");
            if (!result.IsSuccessStatusCode)
            {
                string msg = await result.Content.ReadAsStringAsync();
                throw new Exception(msg);
            }

            Locations? locations = await result.Content.ReadFromJsonAsync<Locations>();
            return locations?.CityLocations?.FirstOrDefault();
        }
    }
}
