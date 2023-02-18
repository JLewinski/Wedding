using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var guests = await _context.Guests.ToListAsync();
            return View(guests);
        }

        [HttpGet]
        public IActionResult Rsvp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rsvp(RsvpViewModel viewModel)
        {
            //TODO: check if someone with that name/email/number is already in
            if (ModelState.IsValid)
            {
                if (await _context.Guests.AnyAsync(x => x.GuestName == viewModel.Name || x.Email == viewModel.Email || x.PhoneNumber == viewModel.PhoneNumber))
                {
                    ModelState.AddModelError(string.Empty, "You have already submitted a RSVP. If you think this is wrong or need to change your RSVP please email jacob@lewinskitech.com");
                }
            }
            if (ModelState.IsValid)
            {
                var password = Guid.NewGuid().ToString();
                var user = new User
                {
                    UserId = Guid.NewGuid(),
                    UserName = viewModel.Email,
                    HashedPassword = BCrypt.Net.BCrypt.HashPassword(password),
                    IsAdmin = false
                };
                _context.Add(user);
                var guest = new Guest
                {
                    Email = viewModel.Email,
                    GuestName = viewModel.Name,
                    NumberAdults = viewModel.NumberAttending,
                    PhoneNumber = viewModel.PhoneNumber,
                    NumberChildren = 0,
                    IsGoing = true,
                    UserId = user.UserId
                };
                _context.Add(guest);
                await _context.SaveChangesAsync();

                //TODO: sign user in and ask for more info (children, address, etc.)
                //TODO: ask user to create password (include in table that this is the initial password)
                //TODO: once emails are working enable a reset password email
                //TODO: create page for guests to message the bride and groom
                //TODO: show link for registry
                var thankYou = new ThankYouViewModel(guest);
                await Services.EmailService.SendConfirmationEmail(thankYou, Url.Action("ThankYou", "Guest", thankYou));

                return RedirectToAction(nameof(ThankYou), thankYou);
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Change(Guid id)
        {
            //var guest = await _context.Guests.FirstOrDefaultAsync(x => x.UserId == id);
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
            {
                return RedirectToAction("hacker", "home");
            }
            return View(new ChangeViewModel(guest));
        }

        [HttpPost]
        public async Task<IActionResult> Change(ChangeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var guest = await _context.Guests.FindAsync(viewModel.UserId);
                guest.NumberChildren = viewModel.NumberChildren;
                guest.NumberAdults = viewModel.NumberAdults;
                guest.PhoneNumber = viewModel.PhoneNumber;
                guest.Email = viewModel.Email;
                guest.IsGoing = viewModel.NumberAdults + viewModel.NumberChildren > 0;
                guest.GuestName = viewModel.GuestName;

                await _context.SaveChangesAsync();
                var thankYou = new ThankYouViewModel(guest);
                await Services.EmailService.SendConfirmationEmail(thankYou, Url.Action("ThankYou", "Guest", thankYou));
                return RedirectToAction(nameof(ThankYou), thankYou);
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Note(Guid id)
        {
            var guest = await _context.Guests.FindAsync(id);
            if(guest == null)
            {
                return RedirectToAction(nameof(HomeController.Hacker), "Home");
            }
            return View(new NoteViewModel { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> Note(NoteViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var guest = await _context.Guests.FindAsync(viewModel.Id);
                if (guest != null)
                {
                    _context.Notes.Add(new Data.Note
                    {
                        NoteText = viewModel.NoteText,
                        GuestId = guest.UserId
                    });

                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(ThankYou));
        }

        public IActionResult ThankYou(ThankYouViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}