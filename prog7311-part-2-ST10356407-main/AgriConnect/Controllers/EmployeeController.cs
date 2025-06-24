using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using AgriConnect.Data;
using AgriConnect.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;


////Tutorialsteacher.com. (2019).Controller in ASP.NET MVC. [online] Available at: https://www.tutorialsteacher.com/mvc/mvc-controller.
public class EmployeeController : Controller
{
    private readonly ApplicationDbContext _context;

    public EmployeeController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Employee/EmployeeDashboard
    [HttpGet]
    public IActionResult EmployeeDashboard(string nameFilter, string categoryFilter, DateTime? productionDateFilter)
    {
        if (HttpContext.Session.GetString("Role") != "Employee")
            return Unauthorized();

        var farmersQuery = _context.Farmers
            .Include(f => f.Products)
            .AsNoTracking()
            .AsQueryable();

        // Filter by Name
        if (!string.IsNullOrWhiteSpace(nameFilter))
            farmersQuery = farmersQuery.Where(f => f.Name.ToLower().Contains(nameFilter.ToLower()));

        // Filter by Product Category (only farmers with at least one matching product)
        if (!string.IsNullOrWhiteSpace(categoryFilter))
            farmersQuery = farmersQuery.Where(f => f.Products.Any(p => p.Category.ToLower().Contains(categoryFilter.ToLower())));

        // Filter by Production Date (only farmers with at least one matching product)
        if (productionDateFilter.HasValue)
        {
            var selectedDate = productionDateFilter.Value.Date;
            farmersQuery = farmersQuery.Where(f => f.Products.Any(p => p.ProductionDate.HasValue && p.ProductionDate.Value.Date == selectedDate));
        }

        var farmers = farmersQuery.ToList();

        // Further filter each farmer's products after retrieval
        foreach (var farmer in farmers)
        {
            farmer.Products = farmer.Products.Where(p =>
                (string.IsNullOrWhiteSpace(categoryFilter) || p.Category.ToLower().Contains(categoryFilter.ToLower())) &&
                (!productionDateFilter.HasValue || (p.ProductionDate.HasValue && p.ProductionDate.Value.Date == productionDateFilter.Value.Date))
            ).ToList();
        }

        // Remove farmers who have no matching products after filter
        if (!string.IsNullOrWhiteSpace(categoryFilter) || productionDateFilter.HasValue)
        {
            farmers = farmers.Where(f => f.Products.Any()).ToList();
        }

        return View("~/Views/Home/EmployeeDashboard.cshtml", farmers);
    }

    // GET: Employee/EmployeeAddFarmer
    [HttpGet]
    public IActionResult EmployeeAddFarmer()
    {
        if (HttpContext.Session.GetString("Role") != "Employee")
            return Unauthorized();

        return View("~/Views/Home/EmployeeAddFarmer.cshtml");
    }

    // POST: Employee/EmployeeAddFarmer
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EmployeeAddFarmer(string username, string password, string name, string email)
    {
        if (HttpContext.Session.GetString("Role") != "Employee")
            return Unauthorized();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
        {
            ModelState.AddModelError("", "All fields are required.");
            return View("~/Views/Home/EmployeeAddFarmer.cshtml");
        }

        if (_context.Users.Any(u => u.Username == username))
        {
            ModelState.AddModelError("Username", "Username already exists.");
            return View("~/Views/Home/EmployeeAddFarmer.cshtml");
        }

        try
        {
            var user = new User
            {
                Username = username,
                Email = email,
                Role = "Farmer"
            };

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, password);

            _context.Users.Add(user);
            _context.SaveChanges();

            var farmer = new Farmer
            {
                UserId = user.UserId,
                Name = name,
                Email = email
            };

            _context.Farmers.Add(farmer);
            _context.SaveChanges();

            TempData["Success"] = "Farmer added successfully.";
            return RedirectToAction("EmployeeDashboard");
        }
        catch
        {
            ModelState.AddModelError("", "An error occurred while adding the farmer. Please try again.");
            return View("~/Views/Home/EmployeeAddFarmer.cshtml");
        }
    }
}
