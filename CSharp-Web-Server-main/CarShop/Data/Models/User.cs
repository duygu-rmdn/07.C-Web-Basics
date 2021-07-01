using System;
using System.ComponentModel.DataAnnotations;

namespace CarShop.Data.Models
{
    public class User
    {
        [Key]
        [Required]
        [MaxLength(40)]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsMechanic { get; set; }
    }
}
