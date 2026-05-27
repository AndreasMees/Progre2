using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data
{
    [Index(nameof(Name), IsUnique = true)]
    public class OperationType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Operation Type")]
        public string Name { get; set; }

        // Navigation property - nullable
        public ICollection<Operation>? Operations { get; set; }
    }
}