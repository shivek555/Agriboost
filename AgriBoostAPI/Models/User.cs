﻿using System.ComponentModel.DataAnnotations;

namespace AgriBoostAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } 

        [Required]
        public string UserType { get; set; } 
    }
}
