using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCNewsSite.Models;
using MVCNewsSite.Services;


namespace MVCNewsSite.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        AppConfiguration _config;
        public string Country { get; set; }
        public string Category { get; set; }

        public string Section { get; set; }

        public NewsController(INewsService newsService,AppConfiguration config) 
        {
            _newsService = newsService;
            this._config = config;
            

        }

        public async Task<IActionResult> Index() 
        {
            List<NewsArticle> newsArticles = new List<NewsArticle>();

            Country = "us";

            string section = (string)RouteData.Values["controller"];
            ViewBag.Section = section;

            

            newsArticles = await _newsService.GetCurrentNewsByCountry(Country, this._config.ApiKey);

            return View("Index",newsArticles);
        
        }

        public async Task<IActionResult> Sports() 
        {

            List<NewsArticle> newsArticles = new List<NewsArticle>();
            Section = (string)RouteData.Values["controller"];
            Category = (string)RouteData.Values["action"].ToString().ToLower();
            Country = "us";
            ViewBag.Section = Section;

            newsArticles = await _newsService.GetTopicalNews(Country, Category, this._config.ApiKey);

            return View("Index",newsArticles);

        }

        public async Task<IActionResult> Science()
        {

            List<NewsArticle> newsArticles = new List<NewsArticle>();
            Section = (string)RouteData.Values["controller"];
            Category = (string)RouteData.Values["action"].ToString().ToLower();
            Country = "us";
            ViewBag.Section = Section;

            newsArticles = await _newsService.GetTopicalNews(Country, Category, this._config.ApiKey);

            return View("Index", newsArticles);

        }

        public async Task<IActionResult> Health()
        {

            List<NewsArticle> newsArticles = new List<NewsArticle>();
            Section = (string)RouteData.Values["controller"];
            Category = (string)RouteData.Values["action"].ToString().ToLower();
            Country = "us";
            ViewBag.Section = Section;

            newsArticles = await _newsService.GetTopicalNews(Country, Category, this._config.ApiKey);

            return View("Index", newsArticles);

        }

        public async Task<IActionResult> Technology()
        {

            List<NewsArticle> newsArticles = new List<NewsArticle>();
            Section = (string)RouteData.Values["controller"];
            Category = (string)RouteData.Values["action"].ToString().ToLower();
            Country = "us";
            ViewBag.Section = Section;

            newsArticles = await _newsService.GetTopicalNews(Country, Category, this._config.ApiKey);

            return View("Index", newsArticles);

        }

        public async Task<IActionResult> Entertainment()
        {

            List<NewsArticle> newsArticles = new List<NewsArticle>();
            Section = (string)RouteData.Values["controller"];
            Category = (string)RouteData.Values["action"].ToString().ToLower();
            Country = "us";
            ViewBag.Section = Section;

            newsArticles = await _newsService.GetTopicalNews(Country, Category, this._config.ApiKey);

            return View("Index", newsArticles);

        }

        public async Task<IActionResult> Business()
        {

            List<NewsArticle> newsArticles = new List<NewsArticle>();
            Section = (string)RouteData.Values["controller"];
            Category = (string)RouteData.Values["action"].ToString().ToLower();
            Country = "us";
            ViewBag.Section = Section;

            newsArticles = await _newsService.GetTopicalNews(Country, Category, this._config.ApiKey);

            return View("Index", newsArticles);

        }

    }
}
