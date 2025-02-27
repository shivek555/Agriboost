using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriBoostAPI.Models
{
    public class Price
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ProductType { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal HistoricalPrice { get; set; }

        [Required]
        public int Year { get; set; }  // ✅ Year column for time-series prediction
    }
}
