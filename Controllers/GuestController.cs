using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wedding.Data;
using Wedding.Models;
using Wedding.Services;

namespace Wedding.Controllers
{
    public class GuestController : Controller
    {
        private readonly ILogger<GuestController> _logger;
        private readonly WeddingContext _context;
        private readonly EmailService _email;

        public GuestController(ILogger<GuestController> logger, WeddingContext context, EmailService emailService)
        {
            _logger = logger;
            _context = context;
            _email = emailService;
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
                if(viewModel.Email == "jacob@lewinskitech.com")
                {
                    //Force to make new user if I RSVP for someone
                    viewModel.Email = string.Empty;
                    if (viewModel.PhoneNumber == "2562034011")
                    {
                        //Force to make new user if I RSVP for someone
                        viewModel.PhoneNumber = string.Empty;
                    }
                }
                else if (viewModel.PhoneNumber == "2562034011")
                {
                    //Force to make new user if I RSVP for someone
                    viewModel.PhoneNumber = string.Empty;
                }
                else if (await _context.Guests.AnyAsync(x => x.GuestName == viewModel.Name || x.Email == viewModel.Email || x.PhoneNumber == viewModel.PhoneNumber))
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
                //TODO: once emails are working enable a reset password email
                //TODO: create page for guests to message the bride and groom
                //TODO: show link for registry
                var thankYou = new ThankYouViewModel(guest);
                var body = await EmailService.RenderViewToStringAsync("ThankYou", thankYou, ControllerContext);
                await _email.SendConfirmationEmail(thankYou, body, Url.Action("ThankYou", "Guest", thankYou));
                

                return RedirectToAction(nameof(ThankYou), thankYou);
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Change(Guid id)
        {
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
                if(guest == null){
                    return RedirectToAction("Index", "Home");
                }
                guest.NumberChildren = viewModel.NumberChildren;
                guest.NumberAdults = viewModel.NumberAdults;
                guest.PhoneNumber = viewModel.PhoneNumber;
                guest.Email = viewModel.Email;
                guest.IsGoing = viewModel.NumberAdults + viewModel.NumberChildren > 0;
                guest.GuestName = viewModel.GuestName;

                await _context.SaveChangesAsync();
                var thankYou = new ThankYouViewModel(guest);
                var body = await EmailService.RenderViewToStringAsync("ThankYou", thankYou, ControllerContext);
                await _email.SendConfirmationEmail(thankYou, body, Url.Action("ThankYou", "Guest", thankYou));
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
                    _context.Notes.Add(new Note
                    {
                        NoteText = viewModel.NoteText,
                        GuestId = guest.UserId,
                        DateCreated = DateTime.Now
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