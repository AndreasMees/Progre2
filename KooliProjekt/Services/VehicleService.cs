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
            return await _uow.VehicleRepository.List(page, pageSize, search);
        }

        public async Task<Vehicle> Get(int id)
        {
            return await _uow.VehicleRepository.Get(id);
        }

        public async Task Save(Vehicle vehicle)
        {
            await _uow.BeginTransaction();
            try
            {
                await _uow.VehicleRepository.Save(vehicle);
                await _uow.Commit();
            }
            catch
            {
                await _uow.Rollback();
                throw;
            }
        }

        public async Task Delete(int id)
        {
            await _uow.BeginTransaction();
            try
            {
                await _uow.VehicleRepository.Delete(id);
                await _uow.Commit();
            }
            catch
            {
                await _uow.Rollback();
                throw;
            }
        }
    }
}