using System.ComponentModel.DataAnnotations;

namespace AgriBoostAPI.Models
{
    public class Buyer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Location { get; set; }
    }
}

