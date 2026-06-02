using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.PublicAPI
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tootja väli on kohustuslik!")]
        [StringLength(50, ErrorMessage = "Tootja nimi ei tohi olla pikem kui 50 sümbolit!")]
        public string? Manufacturer { get; set; }

        [Required(ErrorMessage = "Mudeli väli on kohustuslik!")]
        [StringLength(50, ErrorMessage = "Mudeli nimi ei tohi olla pikem kui 50 sümbolit!")]
        public string? Model { get; set; }

        [Required(ErrorMessage = "Numbrimärk on kohustuslik!")]
        [RegularExpression(@"^[0-9]{3}[A-Z]{3}$", ErrorMessage = "Numbrimärk peab olema kujul 123XYZ!")]
        public string? LicensePlate { get; set; }
    }
}