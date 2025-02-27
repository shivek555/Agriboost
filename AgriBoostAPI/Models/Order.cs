using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AgriBoostAPI.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BuyerId { get; set; }

        [ForeignKey("BuyerId")]
        [JsonIgnore] 
        public Buyer? Buyer { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [JsonIgnore] 
        public Product? Product { get; set; }

        [Required]
        public int SellerId { get; set; }

        [ForeignKey("SellerId")]
        [JsonIgnore] 
        public Seller? Seller { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }
}
