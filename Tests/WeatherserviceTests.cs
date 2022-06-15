using System;
using Xunit;
using Services;

namespace Tests
{
	public class WeatherserviceTests
	{
		[Fact]
		public void GetWeatherDataTest()
		{
			//Arrange:
			WeatherService weatherService = new();

			//Act:
			string response = weatherService.GetWeather();

			//Assert:
			Assert.EndsWith("°C", response);

		}
	}
}