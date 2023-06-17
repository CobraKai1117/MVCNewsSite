using MVCNewsSite.Models;

namespace MVCNewsSite.Services
{
    public interface INewsService
    {

        Task<List<NewsArticle>> GetCurrentNews(string country, string apiKey);

        Task<List<NewsArticle>> GetTopicalNews(string category, string apiKey);


    }



}
