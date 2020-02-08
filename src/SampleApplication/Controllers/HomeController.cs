using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleApplication.Configuration;
using SampleApplication.Models;

namespace SampleApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SampleSettings _settings;
        private readonly LinkSettings _links;

        public HomeController(ILogger<HomeController> logger, SampleSettings settings, LinkSettings links)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(settings));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _links = links ?? throw new ArgumentNullException(nameof(links));
        }

        public IActionResult Index()
        {
            ViewData["Settings"] = _settings;
            ViewData["Links"] = _links;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
