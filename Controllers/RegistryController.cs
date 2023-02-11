using Microsoft.AspNetCore.Mvc;

namespace Wedding.Controllers
{
    public class RegistryController : Controller
    {
        private readonly ILogger<RegistryController> _logger;

        public RegistryController(ILogger<RegistryController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
