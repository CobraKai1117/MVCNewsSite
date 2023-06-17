using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCNewsSite.Models;
using MVCNewsSite.Services;

namespace MVCNewsSite.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        public NewsController(INewsService newsService) 
        {
            _newsService = newsService;
        }

        public async Task<IActionResult> Index(string country = "us", string apiKey = "06d91db4e3444c0d8f2d08e8c743017d") 
        {
            List<NewsArticle> newsArticles = new List<NewsArticle>();

            newsArticles = await _newsService.GetCurrentNews(country, apiKey);

            return View(newsArticles);
        
        }

    }
}
