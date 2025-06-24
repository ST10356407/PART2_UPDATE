using Microsoft.AspNetCore.Mvc;
using AgriConnect.Data;
using AgriConnect.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace AgriConnect
{
    //  //Tutorialsteacher.com. (2019).Controller in ASP.NET MVC. [online] Available at: https://www.tutorialsteacher.com/mvc/mvc-controller.
    public class AuthenticationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthenticationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Authentication/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Home/Login.cshtml");
        }

        // POST: Authentication/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null || new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Failed)
            {
                ViewBag.Error = "Invalid login.";
                return View("~/Views/Home/Login.cshtml");
            }

            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetInt32("UserId", user.UserId);

            if (user.Role == "Farmer")
            {
                return RedirectToAction("FarmerDashboard", "Farmer");
            }
            else if (user.Role == "Employee")
            {
                return RedirectToAction("EmployeeDashboard", "Employee");
            }

            return RedirectToAction("Login");
        }

        // GET: Authentication/Register
        [HttpGet]
        public IActionResult Register()
        {
   
            TempData.Remove("FarmerSuccess");
            TempData.Remove("ProductSuccess");
            TempData.Remove("Success"); 

            return View("~/Views/Home/Register.cshtml", new User());
        }

        // POST: Authentication/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Home/Register.cshtml", model);
            }

            if (_context.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return View("~/Views/Home/Register.cshtml", model);
            }

            var passwordHasher = new PasswordHasher<User>();
            var hashedPassword = passwordHasher.HashPassword(model, model.PasswordHash);

            var newUser = new User
            {
                Username = model.Username,
                PasswordHash = hashedPassword,
                Role = model.Role,
                Email = model.Email
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            // Auto-create Farmer profile if registering as Farmer
            if (newUser.Role == "Farmer")
            {
                var farmer = new Farmer
                {
                    Name = newUser.Username,
                    Email = newUser.Email,
                    UserId = newUser.UserId
                };

                _context.Farmers.Add(farmer);
                _context.SaveChanges();
            }

            TempData["RegisterSuccess"] = "Registration successful. Please log in.";
            return RedirectToAction("Login");
        }

        // GET: Authentication/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
