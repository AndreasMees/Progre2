using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Data
{
    public class Invoice : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "Invoice Number")]
        public string InvoiceNo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Shipping { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Grand Total")]
        public decimal GrandTotal { get; set; }

        // Foreign key
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        // Navigation properties - nullable
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        public ICollection<InvoiceLine>? InvoiceLines { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (InvoiceDate > DateTime.Now)
            {
                yield return new ValidationResult(
                    "Invoice date cannot be in the future.",
                    new[] { nameof(InvoiceDate) });
            }
            if (DueDate < DateTime.Now)
            {
                yield return new ValidationResult(
                    "Due date cannot be in the past.",
                    new[] { nameof(DueDate) });
            }
            if (InvoiceDate > DueDate)
            {
                yield return new ValidationResult(
                    "Due date must be on or after invoice date.",
                    new[] { nameof(DueDate) });
            }
        }
    }
}