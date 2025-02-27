using System.ComponentModel.DataAnnotations;

namespace AgriBoostAPI.Models
{
    public class BuyerProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }  // FK from User

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        public string PreferredProducts { get; set; } // "Fruits, Vegetables"
    }
}

