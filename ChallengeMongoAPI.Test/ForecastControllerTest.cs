using ChallengeMongoAPI.Controllers;
using ChallengeMongoAPI.Models;
using ChallengeMongoAPI.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeMongoAPI.Test
{
    public class ForecastControllerTest
    {
        private readonly ForecastController _controller;
        private readonly IForecastLocalService _localService;
        private readonly IForecastRemoteService _remoteService;
        private float validLatitude = 20.625F;
        private float validLongitude = -103.375F;

        public ForecastControllerTest()
        {
            _localService = A.Fake<IForecastLocalService>();
            _remoteService = A.Fake<IForecastRemoteService>();
            _controller = new ForecastController(_localService, _remoteService);
        }

        #region GetByLatLon

        [Fact]
        public void Get_InvalidLatLon_Should() {
            //Arrange
            var invalidLatitude = 91F;
            var invalidLongitude = 181F;

            //Act
            var actionResult = _controller.Get(invalidLatitude, invalidLongitude);

            //Assert
            var result = actionResult.Result as BadRequestObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal("Invalid latitud or longitude.", result.Value?.ToString());
        }

        [Fact]
        public void Get_ValidLatLon_LocalServiceFound_Should()
        {
            //Arrange
            var validCity = "TestCity";
            var validForecast = A.Fake<Forecast>();
            validForecast.City = validCity;
            A.CallTo(() => _localService.Get(validLatitude, validLongitude)).Returns(validForecast);

            //Act
            var actionResult = _controller.Get(validLatitude, validLongitude);

            //Assert
            var result = actionResult.Result as OkObjectResult;
            var forecast = result.Value as Forecast;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(validCity, forecast.City);
        }

        [Fact]
        public void Get_ValidLatLon_RemoteServiceFound_Should()
        {
            //Arrange
            var validCityRemote = "TestCityRemote";
            Forecast invalidLocalForecast = null;
            var validForecastRemote = A.Fake<Forecast>();
            validForecastRemote.City = validCityRemote;

            A.CallTo(() => _localService.Get(validLatitude, validLongitude)).Returns(invalidLocalForecast);
            A.CallTo(() => _remoteService.Get(validLatitude, validLongitude)).Returns(validForecastRemote);
            A.CallTo(() => _localService.CreateAsync(validForecastRemote)).Returns(validForecastRemote);

            //Act
            var actionResult = _controller.Get(validLatitude, validLongitude);

            //Assert
            var result = actionResult.Result as OkObjectResult;
            var forecast = result.Value as Forecast;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(validCityRemote, forecast.City);
        }

        [Fact]
        public void Get_ValidLatLon_RemoteServiceNotFound_Should()
        {
            //Arrange
            Forecast nullForecast = null;

            A.CallTo(() => _localService.Get(validLatitude, validLongitude)).Returns(nullForecast);
            A.CallTo(() => _remoteService.Get(validLatitude, validLongitude)).Returns(nullForecast);

            //Act
            var actionResult = _controller.Get(validLatitude, validLongitude);

            //Assert
            var result = actionResult.Result as NotFoundObjectResult;
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.Equal("Error getting forecast!", result.Value.ToString());
        }

        #endregion

        #region GetByCity

        [Theory]
        [InlineData(StatusCodes.Status400BadRequest, null)]
        [InlineData(StatusCodes.Status400BadRequest, "")]
        public void GetByCity_InvalidCity_Should(object expected, string cityParam)
        {
            //Arrange
            //Act
            var actionResult = _controller.GetByCity(cityParam);

            //Assert
            var result = actionResult.Result as BadRequestObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, expected);
            Assert.Equal("Invalid city name.", result.Value?.ToString());
        }

        [Fact]
        public void GetByCity_ValidCity_LocalServiceFound_Should() {
            //Arrange
            var validCity = "TestValidCity";
            var validForecast = A.Fake<Forecast>();
            validForecast.City = validCity;

            A.CallTo(() => _localService.GetByCity(validCity)).Returns(validForecast);

            //Act
            var actionResult = _controller.GetByCity(validCity);

            //Assert
            var result = actionResult.Result as OkObjectResult;
            var forecast = result.Value as Forecast;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(validCity, forecast.City);
        }

        [Fact]
        public void GetByCity_ValidCity_RemoteServiceFoundLocation_Should() {
            //Arrange
            var validCityRemote = "TESTVALIDCITYREMOTE";
            Forecast invalidLocalForecast = null;
            var validForecastRemote = A.Fake<Forecast>();
            validForecastRemote.City = validCityRemote;
            var validLocationsRemote = A.Fake<CityLocation>();
            validLocationsRemote.Name = validCityRemote;
            validLocationsRemote.Latitude = validLatitude;
            validLocationsRemote.Longitude = validLongitude;

            A.CallTo(() => _localService.GetByCity(validCityRemote)).Returns(invalidLocalForecast);
            A.CallTo(() => _remoteService.GetCityLocation(validCityRemote)).Returns(validLocationsRemote);
            A.CallTo(() => _remoteService.Get(
                validLocationsRemote.Latitude, 
                validLocationsRemote.Longitude)
            ).Returns(validForecastRemote);
            A.CallTo(() => _localService.CreateAsync(validForecastRemote)).Returns(validForecastRemote);

            //Act
            var actionResult = _controller.GetByCity(validCityRemote);

            //Assert
            var result = actionResult.Result as OkObjectResult;
            var forecast = result.Value as Forecast;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(validCityRemote, forecast.City);
        }

        [Fact]
        public void GetByCity_ValidCity_RemoteServiceNotFoundLocation_Should()
        {
            //Arrange
            var validCityRemote = "TestValidCityRemote";
            Forecast invalidLocalForecast = null;
            var validForecastRemote = A.Fake<Forecast>();
            validForecastRemote.City = validCityRemote;
            CityLocation invalidLication = null;

            A.CallTo(() => _localService.GetByCity(validCityRemote)).Returns(invalidLocalForecast);
            A.CallTo(() => _remoteService.GetCityLocation(validCityRemote)).Returns(invalidLication);

            //Act
            var actionResult = _controller.GetByCity(validCityRemote);

            //Assert
            var result = actionResult?.Result as NotFoundObjectResult;
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.Equal(
                $"The city {validCityRemote} is incorrect or not exist!", 
                result.Value.ToString());
        }

        #endregion
    }
}
