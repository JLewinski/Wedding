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

        [HttpPost]
        public async Task<IActionResult> Rsvp(RsvpViewModel viewModel)
        {
            //TODO: check if someone with that name/email/number is already in
            if (ModelState.IsValid)
            {
                var password = Guid.NewGuid().ToString();
                var user = new User{
                    UserId = Guid.NewGuid(),
                    UserName = viewModel.Email,
                    HashedPassword = BCrypt.Net.BCrypt.HashPassword(password),
                    IsAdmin = false
                };
                _context.Add(user);
                _context.Add(new Guest
                {
                    Email = viewModel.Email,
                    GuestName = viewModel.Name,
                    NumberAdults = viewModel.NumberAttending,
                    PhoneNumber = viewModel.PhoneNumber,
                    NumberChildren = 0,
                    IsGoing = true,
                    UserId = user.UserId
                });
                await _context.SaveChangesAsync();
                //TODO: sign user in and ask for more info (children, address, etc.)
                //TODO: create page for guests to message the bride and groom
                //TODO: show link for registry
                return RedirectToAction(nameof(ThankYou), new ThankYouViewModel{Name = viewModel.Name, Password = password, Username = viewModel.Email});
            }
            return View(viewModel);
        }

        public IActionResult ThankYou(ThankYouViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}