using ChallengeMongoAPI.Models;
using ChallengeMongoAPI.Services.Abstractions;
using ChallengeMongoAPI.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeMongoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForecastController : ControllerBase
    {
        private readonly IForecastLocalService _localService;
        private readonly IForecastRemoteService _remoteService;

        public ForecastController(IForecastLocalService localService, IForecastRemoteService remoteService)
        {
            _localService = localService;
            _remoteService = remoteService;
        }

        // GET: api/forecast
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Forecast))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Forecast> Get([FromQuery] float lat, [FromQuery] float lon)
        {
            if (!Validators.LatLogValidator(lat, lon))
            {
                return BadRequest("Invalid latitud or longitude.");
            }

            var localForecast = _localService.Get(lat, lon);
            if (localForecast == null)
            {
                var remoteForecast = _remoteService.Get(lat, lon).Result;
                if (remoteForecast == null)
                {
                    return NotFound("Error getting forecast!");
                }

                _localService.CreateAsync(remoteForecast);

                return Ok(remoteForecast);
            }

            return Ok(localForecast);
        }

        // GET api/forecast/city
        [HttpGet("{city}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Forecast))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Forecast> GetByCity(string city)
        {
            if (string.IsNullOrEmpty(city)) {
                return BadRequest("Invalid city name.");
            }

            var localForecast = _localService.GetByCity(city);
            if (localForecast == null)
            {
                var cityLocation = _remoteService.GetCityLocation(city).Result;
                if (cityLocation == null)
                {
                    return NotFound($"The city {city} is incorrect or not exist!");
                }

                var remoteForecast = _remoteService.Get(cityLocation.Latitude, cityLocation.Longitude).Result;
                if (remoteForecast == null)
                {
                    return NotFound("Error getting forecast!");
                }

                remoteForecast.City = city.ToUpper();
                _localService.CreateAsync(remoteForecast);

                return Ok(remoteForecast);

            }

            return Ok(localForecast);
        }
    }
}
