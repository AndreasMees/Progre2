using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class VehiclesIndexModel
    {
        public VehicleSearch Search { get; set; }
        public PagedResult<Vehicle> Data { get; set; }
    }
}