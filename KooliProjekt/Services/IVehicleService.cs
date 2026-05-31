using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IVehicleService
    {
        Task<PagedResult<Vehicle>> List(int page, int pageSize);
        Task<Vehicle> Get(int id);
        Task Save(Vehicle vehicle);
        Task Delete(int id);
    }
}