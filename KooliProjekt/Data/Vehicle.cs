using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data
{
    [Index(nameof(LicensePlate), IsUnique = true)]
    public class Vehicle : Entity
    {
        

        [Required]
        [StringLength(100)]
        [Display(Name = "Manufacturer")]
        public string Manufacturer { get; set; }

        [Required]
        [StringLength(100)]
        public string Model { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "License Plate")]
        [RegularExpression(@"^[A-Z0-9]{1,15}$", ErrorMessage = "License plate can only contain uppercase letters and numbers")]
        public string LicensePlate { get; set; }

        // Navigation property - nullable, ei nõua valideerimist
        public ICollection<Operation>? Operations { get; set; }
    }
}