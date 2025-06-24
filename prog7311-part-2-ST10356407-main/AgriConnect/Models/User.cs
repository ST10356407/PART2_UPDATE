using System.ComponentModel.DataAnnotations;

namespace AgriConnect.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }  
    }
}
//Y42. (2025).Create your first SQL Model. [online] Available at: https://docs.y42.com/docs/create-your-first-sql-model [Accessed 14 May 2025].