using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace AgriConnect.Controllers
{
    //Tutorialsteacher.com. (2019).Controller in ASP.NET MVC. [online] Available at: https://www.tutorialsteacher.com/mvc/mvc-controller.
    public class HomeController : Controller
    {
        // GET: /
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                if (role == "Farmer")
                {
                    return RedirectToAction("Dashboard", "Farmer");
                }
                else if (role == "Employee")
                {
                    return RedirectToAction("Dashboard", "Employee");
                }
            }

            return RedirectToAction("Login");
        }

        // GET: /Home/Login
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Home/Login
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(string username, string password)
        {
            // Dummy login logic for demonstration
            if (username == "farmer" && password == "123")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "farmer"),
                    new Claim(ClaimTypes.Role, "Farmer")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Dashboard", "Farmer");
            }
            else if (username == "employee" && password == "123")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "employee"),
                    new Claim(ClaimTypes.Role, "Employee")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Dashboard", "Employee");
            }

            ViewBag.Error = "Invalid login credentials.";
            return View();
        }

        // GET: /Home/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // GET: /Home/AccessDenied
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
