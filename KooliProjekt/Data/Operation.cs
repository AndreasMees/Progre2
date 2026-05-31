using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace KooliProjekt.Data
{
    public class Operation : Entity
    {
        

        [Required]
        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }

        [Required]
        [Display(Name = "Operation Type")]
        public int OperationTypeId { get; set; }

        [Required]
        [Display(Name = "Assigned Employee")]
        public string AssignedEmployeeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Operation Date")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Status")]
        public OperationStatus Status { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Cost (optional)")]
        public decimal? Cost { get; set; }

        // Navigation properties - nullable
        [ForeignKey("VehicleId")]
        public virtual Vehicle? Vehicle { get; set; }

        [ForeignKey("OperationTypeId")]
        public virtual OperationType? OperationType { get; set; }

        [ForeignKey("AssignedEmployeeId")]
        public virtual IdentityUser? AssignedEmployee { get; set; }
    }
}