using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IVehicleService
    {
        Task<PagedResult<Vehicle>> List(int page, int pageSize, VehicleSearch search = null);
        Task<Vehicle> Get(int id);
        Task<bool> Save(Vehicle vehicle);
        Task<bool> Update(Vehicle vehicle);
        Task<bool> Delete(int id);
    }
}