
using Microsoft.Build.Framework;
using MVCNewsSite.Models;
using System.Net.Http;
using System.Text.Json;
using System.Globalization;

namespace MVCNewsSite.Services
{
    public class NewsService: INewsService
    {
        private static HttpClient _httpClient;


        static NewsService() 
        {

            _httpClient = new HttpClient() { BaseAddress = new Uri("https://newsapi.org/") };
        }

        public async Task<List<NewsArticle>> GetCurrentNewsByCountry(string country, string apiKey)
        {
            

            try
            {
                var url = $"v2/top-headlines?country={country}&apiKey={apiKey}";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("User-Agent", "YourApplicationName");

                var response = await _httpClient.SendAsync(request);

                var result = new List<NewsArticle>();

                if (response.IsSuccessStatusCode)
                {
                    string stringResponse = await response.Content.ReadAsStringAsync();

                    CultureInfo[] test = CultureInfo.GetCultures(CultureTypes.AllCultures);
                   
                    result = JsonSerializer.Deserialize<NewsModel>(stringResponse).articles;
                }
                else
                {
                    Console.WriteLine($"Response status code: {response.StatusCode}");
                    //Console.WriteLine($"Response Body: {response.response}");

                    throw new HttpRequestException($"The API request failed with status code {response.StatusCode}");
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                throw;
            }
        }

        public async Task<List<NewsArticle>> GetTopicalNews(string language, string category, string apiKey)
        {
            //Language = getCountryInformation();
            
            var url = $"v2/top-headlines?country={language}&category={category}&apiKey={apiKey}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "YourApplicationName");


            var response = await _httpClient.SendAsync(request);

            var result = new List<NewsArticle>();

            var sResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<NewsModel>(stringResponse).articles;
            }
            else
            {
                Console.WriteLine($"Response status code: {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"Status Code: {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"Response Body: {sResponse}");
                throw new HttpRequestException($"The API request failed with status code {response.StatusCode}");
            }

            return result;
        }


        public async Task<List<NewsArticle>> GetNewsBySearch(string language, string queryParameter,string apiKey)
        {
            //Language = getCountryInformation();

            var url = $"v2/everything?language={language}&q={queryParameter}&apiKey={apiKey}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "YourApplicationName");


            var response = await _httpClient.SendAsync(request);

            var result = new List<NewsArticle>();

            var sResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<NewsModel>(stringResponse).articles;
                
            }
            else
            {
                Console.WriteLine($"Response status code: {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"Status Code: {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"Response Body: {sResponse}");
                throw new HttpRequestException($"The API request failed with status code {response.StatusCode}");
            }

            return result;
        }

















        public string[] getCountryInformation() // Gets the users country and language 
        {

            string currentLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            string currentCountry = CultureInfo.CurrentCulture.Name.Substring(3, 2).ToLower();
            string[] locationInformation = new string[] { currentLanguage, currentCountry };
            return locationInformation;
        
        }

    }
}
