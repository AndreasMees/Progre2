using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IVehicleService
    {
        Task<PagedResult<Vehicle>> List(int page, int pageSize, VehicleSearch search = null);
        Task<Vehicle> Get(int id);
        Task Save(Vehicle vehicle);
        Task Delete(int id);
    }
}