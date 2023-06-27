using MaxMind.GeoIP2;
using Microsoft.AspNetCore.Mvc;
using MVCNewsSite.Models;
using MVCNewsSite.Services;
using System.Diagnostics;
using System.Globalization;

namespace MVCNewsSite.Controllers
{
    public class NewsController : Controller
    {
        private readonly ILogger<NewsController> _logger;

        private readonly INewsService _newsService;

        private readonly IWeatherService _weatherService;

        private readonly WebServiceClient _maxMindClient;

        List<NewsArticle> newsArticles;

        LocationModel currentWeather;

        AppConfiguration _config;
        public string Country { get; set; }
        public string Category { get; set; }

        public string Section { get; set; }

        string[] userLocalConfigs = new string[2];


        public NewsController(INewsService newsService, IWeatherService weatherService, AppConfiguration config, ILogger<NewsController>logger, WebServiceClient maxMindClient)
        {
            _newsService = newsService;
            _weatherService = weatherService;
            _maxMindClient = maxMindClient;
            this._config = config;
            userLocalConfigs = getCountryInformation();
            Country = userLocalConfigs[1];


        }

        
        [Route("", Name ="News")]
        [Route("News")]
        [Route("News/Home")]
        
        
        public async Task<IActionResult> Index(string latitude,string longitude)
        {
             newsArticles = new List<NewsArticle>();

            //var location = await _maxMindClient.CountryAsync();

            string test1 = latitude;
            string test2 = longitude;

         
            string section = (string)RouteData.Values["controller"];
            ViewBag.Section = section;

            currentWeather = await _weatherService.GetWeatherByLocationAsync("/current.json", this._config.weatherApiKey,"45","90");



            newsArticles = await _newsService.GetCurrentNewsByCountry(Country, this._config.ApiKey);

            return View("Index", newsArticles);

        }
       
        public async Task<IActionResult> Sports(double latitude, double longitude)
        {

            newsArticles = new List<NewsArticle>();
            Section = (string)RouteData.Values["controller"];
            Category = (string)RouteData.Values["action"].ToString().ToLower();
            ViewBag.Section = Section;

            newsArticles = await _newsService.GetTopicalNews(Country, Category, this._config.ApiKey);

            currentWeather = await _weatherService.GetWeatherByLocationAsync("/current.json", this._config.weatherApiKey, "45", "90");

            return View("Index", newsArticles);

        }

        public async Task<IActionResult> Science()
        {

            newsArticles = new List<NewsArticle>();
            Section = (string)RouteData.Values["controller"];
            Category = (string)RouteData.Values["action"].ToString().ToLower();
            ViewBag.Section = Section;

            newsArticles = await _newsService.GetTopicalNews(Country, Category, this._config.ApiKey);

            return View("Index", newsArticles);

        }

        public async Task<IActionResult> Health()
        {

            newsArticles = new List<NewsArticle>();
            Section = (string)RouteData.Values["controller"];
            Category = (string)RouteData.Values["action"].ToString().ToLower();
            ViewBag.Section = Section;

            newsArticles = await _newsService.GetTopicalNews(Country, Category, this._config.ApiKey);

            return View("Index", newsArticles);

        }

        public async Task<IActionResult> Technology()
        {

            newsArticles = new List<NewsArticle>();
            Section = (string)RouteData.Values["controller"];
            Category = (string)RouteData.Values["action"].ToString().ToLower();
            ViewBag.Section = Section;

            newsArticles = await _newsService.GetTopicalNews(Country, Category, this._config.ApiKey);

            return View("Index", newsArticles);

        }

        public async Task<IActionResult> Entertainment()
        {

            newsArticles = new List<NewsArticle>();
            Section = (string)RouteData.Values["controller"];
            Category = (string)RouteData.Values["action"].ToString().ToLower();
            Country = userLocalConfigs[1];
            ViewBag.Section = Section;

            newsArticles = await _newsService.GetTopicalNews(Country, Category, this._config.ApiKey);

            return View("Index", newsArticles);

        }

        public async Task<IActionResult> Business()
        {

            newsArticles = new List<NewsArticle>();
            Section = (string)RouteData.Values["controller"];
            Category = (string)RouteData.Values["action"].ToString().ToLower();
            Country = userLocalConfigs[1];
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

        public string[] getCountryInformation() // Gets the users country and language 
        {

            string currentLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            string currentCountry = CultureInfo.CurrentCulture.Name.Substring(3, 2).ToLower();
            string[] locationInformation = new string[] { currentLanguage, currentCountry };
            return locationInformation;

        }
    }
}