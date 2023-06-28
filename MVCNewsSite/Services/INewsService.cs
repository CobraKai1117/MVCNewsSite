using MVCNewsSite.Models;

namespace MVCNewsSite.Services
{
    public interface INewsService
    {

        Task<List<NewsArticle>> GetCurrentNewsByCountry(string country, string apiKey);

        Task<List<NewsArticle>> GetTopicalNews(string country, string category, string apiKey);

        Task<List<NewsArticle>> GetNewsBySearch(string queryParameter,string apiKey);

    }



}
