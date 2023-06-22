using Microsoft.AspNetCore.Mvc;
using MVCNewsSite.Models;
using MVCNewsSite.Services;
using System.Diagnostics;

namespace MVCNewsSite.Controllers
{
    public class NewsController : Controller
    {
        private readonly ILogger<NewsController> _logger;

        private readonly INewsService _newsService;
        AppConfiguration _config;
        public string Country { get; set; }
        public string Category { get; set; }

        public string Section { get; set; }

        
        public NewsController(INewsService newsService, AppConfiguration config, ILogger<NewsController>logger)
        {
            _newsService = newsService;
            this._config = config;


        }

        [Route("", Name ="News")]
        [Route("News")]
        [Route("News/Home")]
        public async Task<IActionResult> Index()
        {
            List<NewsArticle> newsArticles = new List<NewsArticle>();

            Country = "us";

            string section = (string)RouteData.Values["controller"];
            ViewBag.Section = section;



            newsArticles = await _newsService.GetCurrentNewsByCountry(Country, this._config.ApiKey);

            return View("Index", newsArticles);

        }

        public async Task<IActionResult> Sports()
        {

            List<NewsArticle> newsArticles = new List<NewsArticle>();
            Section = (string)RouteData.Values["controller"];
            Category = (string)RouteData.Values["action"].ToString().ToLower();
            Country = "us";
            ViewBag.Section = Section;

            newsArticles = await _newsService.GetTopicalNews(Country, Category, this._config.ApiKey);

            return View("Index", newsArticles);

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

        public IActionResult Privacy()
        {
            string section = (string)RouteData.Values["controller"];
            ViewBag.Section = section;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}