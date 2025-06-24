using AgriConnect.Models;
using System.Collections.Generic;

namespace AgriConnect.Models
{
    public class Farmer
    {
        public int FarmerId { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    

        // Navigation properties
        public User? User { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}

//Y42. (2025).Create your first SQL Model. [online] Available at: https://docs.y42.com/docs/create-your-first-sql-model [Accessed 14 May 2025].