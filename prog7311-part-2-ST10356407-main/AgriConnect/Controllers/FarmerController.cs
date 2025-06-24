using Microsoft.EntityFrameworkCore;
using AgriConnect.Models;
using AgriConnect.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AgriConnect.Controllers
{
    ////Tutorialsteacher.com. (2019).Controller in ASP.NET MVC. [online] Available at: https://www.tutorialsteacher.com/mvc/mvc-controller.
    public class FarmerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FarmerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Farmer Dashboard
        public async Task<IActionResult> FarmerDashboard(string nameFilter, string categoryFilter, DateTime? startDateFilter)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Authentication");

            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == userId.Value);
            if (farmer == null)
            {
                // Redirect to profile creation if not found
                TempData["Error"] = "No farmer profile found. Please create one.";
                return RedirectToAction("CreateProfile");
            }

            var productsQuery = _context.Products
                .Where(p => p.FarmerId == farmer.FarmerId)
                .AsQueryable();

            // Apply optional filters
            if (!string.IsNullOrEmpty(nameFilter))
                productsQuery = productsQuery.Where(p => p.ProductName.Contains(nameFilter));

            if (!string.IsNullOrEmpty(categoryFilter))
                productsQuery = productsQuery.Where(p => p.Category.Contains(categoryFilter));

            if (startDateFilter.HasValue)
                productsQuery = productsQuery.Where(p => p.ProductionDate >= startDateFilter.Value);

            var products = await productsQuery.ToListAsync();

            var viewModel = new FarmerDashboardViewModel
            {
                Farmer = farmer,
                Products = products
            };

            ViewBag.FarmerName = farmer.Name;
            ViewBag.ProductCount = products.Count;

            return View(viewModel);
        }

        // GET: Add Product Page
        [HttpGet]
        public IActionResult FarmerAddProduct()
        {
            return View("~/Views/Home/FarmerAddProduct.cshtml", new Product());
        }

        // POST: Add Product Submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FarmerAddProduct(Product model)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Authentication");

            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == userId.Value);
            if (farmer == null)
                return RedirectToAction("CreateProfile");

            if (!ModelState.IsValid)
                return View("~/Views/Home/FarmerAddProduct.cshtml", model);

            model.FarmerId = farmer.FarmerId;
            model.UserId = userId.Value;

            _context.Products.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Product added successfully!";
            return RedirectToAction("FarmerDashboard");
        }

        // GET: Create Farmer Profile Page
        [HttpGet]
        public IActionResult CreateProfile()
        {
            return View(); // Make sure a View named CreateProfile.cshtml exists
        }

        // POST: Save Farmer Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProfile(Farmer farmer)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Authentication");

            if (!ModelState.IsValid)
                return View(farmer);

            farmer.UserId = userId.Value;

            _context.Farmers.Add(farmer);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Farmer profile created successfully!";
            return RedirectToAction("FarmerDashboard");
        }
    }
}
