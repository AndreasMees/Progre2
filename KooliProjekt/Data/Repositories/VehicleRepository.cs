using KooliProjekt.Search;

namespace KooliProjekt.Data.Repositories
{
    public class VehicleRepository : BaseRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PagedResult<Vehicle>> List(int page, int pageSize, VehicleSearch search = null)
        {
            var query = DbContext.Vehicles.AsQueryable();

            search = search ?? new VehicleSearch();

            if (!string.IsNullOrWhiteSpace(search.Keyword))
            {
                query = query.Where(v => v.Manufacturer.Contains(search.Keyword) ||
                                        v.Model.Contains(search.Keyword) ||
                                        v.LicensePlate.Contains(search.Keyword));
            }

            if (!string.IsNullOrWhiteSpace(search.Manufacturer))
            {
                query = query.Where(v => v.Manufacturer.Contains(search.Manufacturer));
            }

            return await query.OrderByDescending(x => x.Id).GetPagedAsync(page, pageSize);
        }
    }
}