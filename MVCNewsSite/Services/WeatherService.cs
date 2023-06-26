using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVCNewsSite.Models;
using System.Text.Json;

namespace MVCNewsSite.Services
{
    public class WeatherService : IWeatherService
    {
        public static HttpClient _httpClient;

        static WeatherService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://api.weatherapi.com/") };

        }


        public async Task<LocationModel> GetWeatherByLocationAsync(string implementation, string apiKey, string latitude, string longitude) 
        {
            try
            {

                var url = $"v1{implementation}?key={apiKey}&q={latitude},{longitude}"; 
                    //$"auto:ip";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("User-Agent", "YourApplicationName");
                var response = await _httpClient.SendAsync(request);

                var result = new LocationModel();

                if(response.IsSuccessStatusCode) 
                {

                    string stringResponse = await response.Content.ReadAsStringAsync();
                    result = JsonSerializer.Deserialize<LocationModel>(stringResponse);
                
                }

                else 
                {
                    throw new HttpRequestException($"The API request failed with the status code{response.StatusCode}");
                
                }

                return result;
            }

            catch(Exception e) 
            {
                Console.WriteLine("An error occurred:" + e.Message);
                throw;
            
            }

            
        
        }

    }
}
