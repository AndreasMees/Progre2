using KooliProjekt.Search;

namespace KooliProjekt.Data.Repositories
{
    public interface IVehicleRepository
    {
        Task<PagedResult<Vehicle>> List(int page, int pageSize, VehicleSearch search = null);
        Task<Vehicle> Get(int id);
        Vehicle Add(Vehicle vehicle);
        Vehicle Update(Vehicle vehicle);
        Vehicle Remove(Vehicle vehicle);
    }
}