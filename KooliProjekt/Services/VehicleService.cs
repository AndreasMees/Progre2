using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _uow;

        public VehicleService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PagedResult<Vehicle>> List(int page, int pageSize, VehicleSearch search = null)
        {
            return await _uow.Vehicles.List(page, pageSize, search);
        }

        public async Task<Vehicle> Get(int id)
        {
            return await _uow.Vehicles.Get(id);
        }

        public async Task<bool> Save(Vehicle vehicle)
        {
            _uow.Vehicles.Add(vehicle);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(Vehicle vehicle)
        {
            _uow.Vehicles.Update(vehicle);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var vehicle = await _uow.Vehicles.Get(id);
            if (vehicle == null) return false;
            _uow.Vehicles.Remove(vehicle);
            await _uow.SaveChangesAsync();
            return true;
        }
    }
}