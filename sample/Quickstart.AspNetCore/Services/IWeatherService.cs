using System.Threading.Tasks;

namespace Quickstart.AspNetCore.Services
{
    interface IWeatherService
    {
        Task<CurrentWeather> GetWeatherAsync(float lat, float lon);
    }
}