using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriBoostAPI.Models
{
    public class Farmer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string ProductType { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")] // ✅ Ensures correct precision for Price
        public decimal Price { get; set; }
    }
}
