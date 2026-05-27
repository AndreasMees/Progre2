using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public enum OperationStatus
    {
        [Display(Name = "Pending")]
        Pending = 0,
        
        [Display(Name = "In Progress")]
        InProgress = 1,
        
        [Display(Name = "Completed")]
        Completed = 2
    }
}