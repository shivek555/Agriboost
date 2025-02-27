using System.ComponentModel.DataAnnotations;

namespace AgriBoostAPI.Models
{
    public class SellerProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string ShopName { get; set; }

        public string BusinessType { get; set; } // "Retail", "Wholesale"

        public string GSTNumber { get; set; }

        [Required]
        public string Location { get; set; }
    }
}
