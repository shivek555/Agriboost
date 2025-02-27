using System.ComponentModel.DataAnnotations;

namespace AgriBoostAPI.Models
{
    public class FarmerProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public double LandSize { get; set; } // In acres

        [Required]
        public string CropType { get; set; } // "Rice, Wheat"

        public string WaterSource { get; set; } // "Irrigation, Rainwater"

        public int Experience { get; set; } // Years of farming
    }
}
