using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using AgriConnect.Data;
using Microsoft.AspNetCore.Authentication.Cookies;  

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Register DbContext with your SQL Server connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register IHttpContextAccessor to allow using HttpContext.Session
builder.Services.AddHttpContextAccessor();

// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Redirect to login page if not authenticated
        options.LogoutPath = "/Account/Logout"; // Redirect to logout page
        options.AccessDeniedPath = "/Account/AccessDenied"; // Optional, for access denial
    });

// Optional but good for cookie policies in login/logout flows
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable authentication and session before authorization
app.UseAuthentication();  // Add this line to enable authentication middleware
app.UseSession();         // Keep this to enable session support
app.UseAuthorization();   // This line should come after authentication

// Cookie policy middleware (optional)
app.UseCookiePolicy();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
