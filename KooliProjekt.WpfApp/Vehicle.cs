namespace KooliProjekt.WpfApp
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string LicensePlate { get; set; }

        public override string ToString()
        {
            return $"{Manufacturer} {Model} ({LicensePlate})";
        }
    }
}