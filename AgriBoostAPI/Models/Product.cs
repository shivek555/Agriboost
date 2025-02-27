using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AgriBoostAPI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; } // e.g., Vegetables, Fruits, Dairy

        [Required]
        public int SellerId { get; set; }

        [ForeignKey("SellerId")]
        [JsonIgnore] 
        public Seller? Seller { get; set; }
    }
}
