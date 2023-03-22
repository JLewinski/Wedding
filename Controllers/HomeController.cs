using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Wedding.Models;

namespace Wedding.Controllers
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
            try{
                var randomNumberGenerator = new Random();
                var slideshowImages = Directory.GetFiles(@"./wwwroot/dist/img/slideshow")
                    .Select(x =>
                    {
                        int width, height;
                        using(var img = Image.Load(x))
                        {
                            width = img.Width;
                            height = img.Height;
                        }
                        
                        return (x.Substring(9), width <= height);
                    })
                    .OrderBy(x => randomNumberGenerator.Next())
                    .ToList();

                ViewData["slideshowImages"] = slideshowImages;
            }
            catch{

            }
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

        public IActionResult Hacker()
        {
            return View();
        }
    }
}