using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data.Repositories
{
    public class OperationRepository : BaseRepository<Operation>, IOperationRepository
    {
        public OperationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<Operation>> List(int page, int pageSize)
        {
            return await DbContext.Operations
                .Include(o => o.Vehicle)
                .Include(o => o.OperationType)
                .Include(o => o.AssignedEmployee)
                .OrderByDescending(x => x.Id)
                .GetPagedAsync(page, pageSize);
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