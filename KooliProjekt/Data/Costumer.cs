using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data
{
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Phone), IsUnique = true)]
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Customer Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(15)]
        [RegularExpression(@"^\+\d{1,4}\d+$", ErrorMessage = "Phone must start with + and country code (e.g., +37251234567)")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Range(0, 0.9)]
        [Display(Name = "Discount (0-0.9)")]
        public decimal Discount { get; set; }

        // Navigation property - nullable
        public ICollection<Invoice>? Invoices { get; set; }
    }
}