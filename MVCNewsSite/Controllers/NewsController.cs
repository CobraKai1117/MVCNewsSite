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

            string section = (string)RouteData.Values["controller"];
            ViewBag.Section = section;

            newsArticles = await _newsService.GetCurrentNewsByCountry(country, apiKey);

            return View("Index",newsArticles);
        
        }

        public async Task<IActionResult> Sports(string country = "us",string category = "sports", string apiKey = "06d91db4e3444c0d8f2d08e8c743017d") 
        {

            List<NewsArticle> newsArticles = new List<NewsArticle>();
            string section = (string)RouteData.Values["controller"];
            ViewBag.Section = section;

            newsArticles = await _newsService.GetTopicalNews(country, category, apiKey);

            return View("Index",newsArticles);

        }

        public async Task<IActionResult> Science(string country = "us", string category = "science", string apiKey = "06d91db4e3444c0d8f2d08e8c743017d")
        {

            List<NewsArticle> newsArticles = new List<NewsArticle>();
            string section = (string)RouteData.Values["controller"];
            ViewBag.Section = section;

            newsArticles = await _newsService.GetTopicalNews(country, category, apiKey);

            return View("Index", newsArticles);

        }

        public async Task<IActionResult> Health(string country = "us", string category = "health", string apiKey = "06d91db4e3444c0d8f2d08e8c743017d")
        {

            List<NewsArticle> newsArticles = new List<NewsArticle>();
            string section = (string)RouteData.Values["controller"];
            ViewBag.Section = section;

            newsArticles = await _newsService.GetTopicalNews(country, category, apiKey);

            return View("Index", newsArticles);

        }

        public async Task<IActionResult> Technology(string country = "us", string category = "technology", string apiKey = "06d91db4e3444c0d8f2d08e8c743017d")
        {

            List<NewsArticle> newsArticles = new List<NewsArticle>();
            string section = (string)RouteData.Values["controller"];
            ViewBag.Section = section;

            newsArticles = await _newsService.GetTopicalNews(country, category, apiKey);

            return View("Index", newsArticles);

        }

        public async Task<IActionResult> Entertainment(string country = "us", string category = "entertainment", string apiKey = "06d91db4e3444c0d8f2d08e8c743017d")
        {

            List<NewsArticle> newsArticles = new List<NewsArticle>();
            string section = (string)RouteData.Values["controller"];
            ViewBag.Section = section;

            newsArticles = await _newsService.GetTopicalNews(country, category, apiKey);

            return View("Index", newsArticles);

        }

        public async Task<IActionResult> Business(string country = "us", string category = "business", string apiKey = "06d91db4e3444c0d8f2d08e8c743017d")
        {

            List<NewsArticle> newsArticles = new List<NewsArticle>();
            string section = (string)RouteData.Values["controller"];
            ViewBag.Section = section;

            newsArticles = await _newsService.GetTopicalNews(country, category, apiKey);

            return View("Index", newsArticles);

        }

    }
}
