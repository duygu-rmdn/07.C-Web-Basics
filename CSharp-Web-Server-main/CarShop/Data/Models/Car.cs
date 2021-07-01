using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Data.Models
{
    public class Car
    {
        [Key]
        [Required]
        [MaxLength(40)]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(20)]
        public string Model { get; set; }

        public int Year { get; set; }
        
        [Required]
        public string PictureUrl { get; set; }

        [Required]
        [MaxLength(8)]
        public string PlateNumber { get; set; }

        [Required]
        public string OwnerId { get; set; }
        public User Owner { get; set; }
        public IEnumerable<Issue> Issues { get; init; } = new List<Issue>();
    }
}
