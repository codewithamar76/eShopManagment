using eShopFlix.HttpClients;
using eShopFlix.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace eShopFlix.Controllers
{
    public class AccountController : Controller
    {
        AuthServiceClient _authServiceClient;
        public AccountController(AuthServiceClient auth)
        {
            _authServiceClient = auth;
        }
        private void GenerateTicket(UserModel user)
        {
            // Logic to generate a ticket for the user
            // This could involve creating a session or a JWT token
            string strData = JsonSerializer.Serialize(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.UserData, strData),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, string.Join(",", user.Roles))
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Set to true if you want the session to persist across browser sessions
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1) // Set expiration time for the ticket
            };
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity), authProperties).Wait();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _authServiceClient.LoginAsync(loginModel);
                if (user != null)
                {
                    GenerateTicket(user);
                    if(!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    // Set user session or cookie here
                    if (user.Roles.Contains("User"))
                    {
                        return RedirectToAction("Index", "Home", new { area="User"});
                    }
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(loginModel);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Clear user session or cookie here
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult UnAuthorize()
        {
            return View();
        }
    }
}
