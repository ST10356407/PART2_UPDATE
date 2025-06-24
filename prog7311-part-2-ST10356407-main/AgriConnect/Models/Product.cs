using System;
using System.ComponentModel.DataAnnotations;

namespace AgriConnect.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public int FarmerId { get; set; }

        public int? UserId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string Category { get; set; }

        public DateTime? ProductionDate { get; set; }

        public Farmer? Farmer { get; set; }
    }
}
//Y42. (2025).Create your first SQL Model. [online] Available at: https://docs.y42.com/docs/create-your-first-sql-model [Accessed 14 May 2025].