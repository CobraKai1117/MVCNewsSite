
using Microsoft.Build.Framework;
using MVCNewsSite.Models;
using System.Net.Http;
using System.Text.Json;

namespace MVCNewsSite.Services
{
    public class NewsService: INewsService
    {
        private static HttpClient _httpClient;
        static NewsService() 
        {

            _httpClient = new HttpClient() { BaseAddress = new Uri("https://newsapi.org/") };
        }

        public async Task<List<NewsArticle>> GetCurrentNews(string country, string apiKey)
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

        public async Task<List<NewsArticle>> GetTopicalNews(string category, string apiKey)
        {
            var url = string.Format("v2/top-headlines/sources?category={0}&apiKey={1}", category, apiKey);
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "YourApplicationName");
            var result = new List<NewsArticle>();
            var response = await _httpClient.GetAsync(url);
            var sResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<List<NewsArticle>>(stringResponse);
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



    }
}
