using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Wedding.Data;
using Wedding.Models;

namespace Wedding.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly WeddingContext _context;

        public UserController(ILogger<UserController> logger, WeddingContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "/")
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(SignInViewModel viewModel, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = await AuthenticateUser(viewModel.Email, viewModel.Password);
                if (user != null)
                {
                    return await SignIn(user, returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Username or Password is incorrect");
                }
            }
            return View(viewModel);

        }

        public async Task<IActionResult> Logout(string returnUrl)
        {
            await HttpContext.SignOutAsync(returnUrl);
            return RedirectToAction("Index", "Home");
        }

#if DEBUG
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel viewModel)
        {
            var alreadyExistingUser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == viewModel.Email);
            if (ModelState.IsValid && alreadyExistingUser != null)
            {
                ModelState.AddModelError(string.Empty, "A user with that email already exists");
            }
            var guest = await _context.Guests.FirstOrDefaultAsync(x => x.Email == viewModel.Email);
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = viewModel.Email,
                    HashedPassword = BCrypt.Net.BCrypt.HashPassword(viewModel.Password),
                    IsAdmin = false,
                    UserId = Guid.NewGuid()
                };


                if (guest != null)
                {
                    guest.UserId = user.UserId;
                }
                _context.Add(user);
                await _context.SaveChangesAsync();
                
                return await SignIn(user);
            }

            return View(viewModel);

        }
#endif

        private async Task<IActionResult> SignIn(User user, string? returnUrl = null)
        {
            var claims = new List<Claim>{
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "Guest")
                };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            _logger.LogInformation($"{user.UserName} logged in at {DateTime.Now}");

            return LocalRedirect(returnUrl ?? Url.Action("Index", "Home") ?? "/");
        }


        private async Task<User?> AuthenticateUser(string username, string password)
        {
            var users = await _context.Users.Where(x => x.UserName == username).ToListAsync();

            if (users.Count == 1 && BCrypt.Net.BCrypt.Verify(password, users[0]?.HashedPassword))
            {
                return users[0];
            }
            return null;
        }
    }
}