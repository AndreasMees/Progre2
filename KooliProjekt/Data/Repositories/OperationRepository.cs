using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data.Repositories
{
    public class OperationRepository : BaseRepository<Operation>, IOperationRepository
    {
        public OperationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PagedResult<Operation>> List(int page, int pageSize, OperationSearch search = null)
        {
            var query = DbContext.Operations
                .Include(o => o.Vehicle)
                .Include(o => o.OperationType)
                .Include(o => o.AssignedEmployee)
                .AsQueryable();

            search = search ?? new OperationSearch();

            if (!string.IsNullOrWhiteSpace(search.Keyword))
            {
                query = query.Where(o => o.Vehicle.LicensePlate.Contains(search.Keyword) ||
                                        o.OperationType.Name.Contains(search.Keyword));
            }

            if (search.Status != null)
            {
                query = query.Where(o => o.Status == search.Status);
            }

            return await query.OrderByDescending(x => x.Id).GetPagedAsync(page, pageSize);
        }

        public override async Task<Operation> Get(int id)
        {
            return await DbContext.Operations
                .Include(o => o.Vehicle)
                .Include(o => o.OperationType)
                .Include(o => o.AssignedEmployee)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}