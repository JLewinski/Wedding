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
        public async Task<IActionResult> Index(int order = 0)
        {
            IQueryable<Guest> guestQuery = _context.Guests;

            guestQuery = order switch
            {
                0 => guestQuery.OrderByDescending(x => x.DateModified).ThenBy(x => x.GuestName),
                1 => guestQuery.OrderBy(x => x.GuestName),
                2 => guestQuery.OrderByDescending(x => x.IsGoing).ThenBy(x => x.GuestName),
                _ => guestQuery.OrderByDescending(x => x.DateModified)
                                        .ThenBy(x => x.GuestName),
            };

            return View(await guestQuery.ToListAsync());
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
            bool replacedInfo = viewModel.ReplaceFillerInfo();
            if (!replacedInfo && await _context.Guests.AnyAsync(x => x.GuestName == viewModel.Name || x.Email == viewModel.Email || x.PhoneNumber == viewModel.PhoneNumber))
            {
                ModelState.AddModelError(string.Empty, "You have already submitted a RSVP. If you think this is wrong or need to change your RSVP please email jacob@lewinskitech.com");
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
                    IsGoing = viewModel.NumberAttending > 0,
                    UserId = user.UserId,
                    DateModified = DateTime.Now
                };
                _context.Add(guest);
                await _context.SaveChangesAsync();

                //TODO: sign user in and ask for more info (children, address, etc.)
                //TODO: create page for guests to message the bride and groom
                var thankYou = new ThankYouViewModel(guest)
                {
                    Url = Url.Action("ThankYou", "Guest")
                };
                thankYou.Url = Url.Action("ThankYou", "Guest", thankYou);

                if (thankYou.Email != string.Empty)
                {
                    await _email.SendConfirmationEmail(thankYou, ControllerContext);
                    await _email.SendNotificationEmail(thankYou, ControllerContext);
                }

                return RedirectToAction(nameof(ThankYou), thankYou);
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Change(Guid id)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new ChangeViewModel(guest));
        }

        [HttpPost]
        public async Task<IActionResult> Change(ChangeViewModel viewModel)
        {
            viewModel.ReplaceFillerInfo();
            if (ModelState.IsValid)
            {

                var guest = await _context.Guests.FindAsync(viewModel.UserId);
                if (guest == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                guest.NumberChildren = viewModel.NumberChildren;
                guest.NumberAdults = viewModel.NumberAdults;
                guest.PhoneNumber = viewModel.PhoneNumber;
                guest.Email = viewModel.Email;
                guest.IsGoing = viewModel.NumberAdults + viewModel.NumberChildren > 0;
                guest.GuestName = viewModel.Name;

                await _context.SaveChangesAsync();

                //Redirect if it is an Admin changing someone else's response
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index");
                }

                var thankYou = new ThankYouViewModel(guest)
                {
                    Url = Url.Action("ThankYou", "Guest")
                };
                thankYou.Url = Url.Action("ThankYou", "Guest", thankYou);

                if (thankYou.Email != null)
                {
                    var body = await EmailService.RenderViewToStringAsync("ThankYou", thankYou, ControllerContext);
                    //await _email.SendConfirmationEmail(thankYou, body);
                }
                return RedirectToAction(nameof(ThankYou), thankYou);
            }
            return View(viewModel);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var guest = await _context.Guests.FindAsync(id);
                var user = await _context.Users.FindAsync(id);
                if (guest != null)
                {
                    _context.Guests.Remove(guest);
                }
                if (user != null)
                {
                    _context.Users.Remove(user);
                }
                await _context.SaveChangesAsync();
            }
            catch
            {
                return RedirectToAction(nameof(Change), id);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Note(Guid id)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
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

        public IActionResult Notification(ThankYouViewModel viewModel)
        {
            return View(viewModel);
        }

        public IActionResult ThankYou(ThankYouViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}