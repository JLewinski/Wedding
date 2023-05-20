using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Wedding.Models;

namespace Wedding.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Data.WeddingContext _dbContext;

        public HomeController(ILogger<HomeController> logger, Data.WeddingContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSlideshow()
        {
            var currentImages = await _dbContext.ImageLocations.Select(x => x.ImageSource).ToListAsync();
            var slideshowImages = Directory.GetFiles(@"./wwwroot/dist/img/slideshow")
                .Where(x => !currentImages.Contains(x.Substring(9)))
                .Select(x =>
                {
                    int width, height;
                    using (var img = Image.Load(x))
                    {
                        width = img.Width;
                        height = img.Height;
                    }

                    return new Data.ImageLocation
                    {
                        Height = height,
                        Width = width,
                        ImageSource = x.Substring(9)
                    };
                })
                .ToList();
            
            _dbContext.AddRange(slideshowImages);
            await _dbContext.SaveChangesAsync();

            return Json(slideshowImages);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var randomNumberGenerator = new Random();
                var slideshowImages = await _dbContext.ImageLocations.ToListAsync();

                ViewData["slideshowImages"] = slideshowImages
                    .OrderBy(x => randomNumberGenerator.Next())
                    .ToList();
            }
            catch
            {

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

        [Route("54DayNovena")]
        public IActionResult Novena(){
            return View();
        }

        [Route("rd")]
        public IActionResult RehearsalDinnerRedirect()
        {
            return RedirectToActionPermanent("RehearsalDinner");
        }

        public IActionResult RehearsalDinner()
        {
            return View();
        }
    }
}