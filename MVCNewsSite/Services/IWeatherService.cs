using MVCNewsSite.Models;

namespace MVCNewsSite.Services
{
    public interface IWeatherService
    {

        Task<LocationModel> GetWeatherByLocationAsync(string implementation, string apiKey, string? latitude, string? longitude);


    }
}
