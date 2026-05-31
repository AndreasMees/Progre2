using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Data
{
    public class InvoiceLine : Entity
    {
        

        [Required]
        [StringLength(255)]
        [Display(Name = "Line Item")]
        public string LineItem { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public decimal Quantity { get; set; }

        [Required]
        [Range(0, 0.5, ErrorMessage = "VAT rate must be between 0 and 0.5")]
        [Display(Name = "VAT Rate")]
        public decimal VatRate { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        // Foreign key
        [Display(Name = "Invoice")]
        public int InvoiceId { get; set; }

        // Navigation property - nullable
        [ForeignKey("InvoiceId")]
        public virtual Invoice? Invoice { get; set; }
    }
}