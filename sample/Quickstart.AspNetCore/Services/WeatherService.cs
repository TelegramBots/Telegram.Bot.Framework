using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Quickstart.AspNetCore.Services
{
    class WeatherService : IWeatherService
    {
        private readonly HttpClient _client;

        public WeatherService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://www.metaweather.com/api/")
            };
        }

        public async Task<CurrentWeather> GetWeatherAsync(float lat, float lon)
        {
            string location = await FindLocationIdAsync(lat, lon)
                .ConfigureAwait(false);

            DateTime today = DateTime.Today;

            string json = await _client.GetStringAsync($"location/{location}/{today.Year}/{today.Month}/{today.Day}")
                .ConfigureAwait(false);

            dynamic arr = JsonConvert.DeserializeObject(json);

            return new CurrentWeather
            {
                Status = arr[0].weather_state_name,
                Temp = arr[0].the_temp,
                MinTemp = arr[0].min_temp,
                MaxTemp = arr[0].max_temp,
            };
        }

        private async Task<string> FindLocationIdAsync(float lat, float lon)
        {
            string json = await _client.GetStringAsync($"location/search?lattlong={lat},{lon}")
                .ConfigureAwait(false);
            dynamic arr = JsonConvert.DeserializeObject(json);
            return arr[0].woeid;
        }
    }
}
