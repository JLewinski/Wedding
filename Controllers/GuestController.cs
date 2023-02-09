using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Wedding.Data;
using Wedding.Models;

namespace Wedding.Controllers
{
    public class GuestController : Controller
    {
        private readonly ILogger<GuestController> _logger;
        private readonly WeddingContext _context;

        public GuestController(ILogger<GuestController> logger, WeddingContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Rsvp()
        {
            return View();
        }

        public async Task<IActionResult> Rsvp(RsvpViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(new Guest
                {
                    Email = viewModel.Email,
                    GuestName = viewModel.Name,
                    NumberAdults = viewModel.NumberAttending,
                    NumberChildren = 0,
                    IsGoing = true
                });
                await _context.SaveChangesAsync();
                return RedirectToAction("ThankYou");
            }
            return View(viewModel);
        }
    }
}