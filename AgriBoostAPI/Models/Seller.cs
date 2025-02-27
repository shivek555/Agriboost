using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgriBoostAPI.Models
{
    public class Seller
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Location { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}
