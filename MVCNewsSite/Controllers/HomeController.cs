using Microsoft.AspNetCore.Mvc;
using MVCNewsSite.Models;
using System.Diagnostics;

namespace MVCNewsSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string section = (string)RouteData.Values["controller"];
            ViewBag.Section = section;
            return View();
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