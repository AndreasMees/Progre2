using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _context;

        public VehicleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Vehicle>> List(int page, int pageSize)
        {
            return await _context.Vehicles.GetPagedAsync(page, pageSize);
        }

        public async Task<Vehicle> Get(int id)
        {
            return await _context.Vehicles.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(Vehicle vehicle)
        {
            if (vehicle.Id == 0)
                _context.Add(vehicle);
            else
                _context.Update(vehicle);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }
    }
}