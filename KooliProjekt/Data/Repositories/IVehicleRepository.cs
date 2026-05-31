namespace KooliProjekt.Data.Repositories
{
    public interface IVehicleRepository
    {
        Task<PagedResult<Vehicle>> List(int page, int pageSize);
        Task<Vehicle> Get(int id);
        Task Save(Vehicle vehicle);
        Task Delete(int id);
    }
}