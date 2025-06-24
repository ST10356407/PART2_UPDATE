using AgriConnect.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AgriConnect.Data;
public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Farmer> Farmers { get; set; }
    public DbSet<Product> Products { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}
