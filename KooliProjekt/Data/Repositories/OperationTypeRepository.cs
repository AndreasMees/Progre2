using KooliProjekt.Search;

namespace KooliProjekt.Data.Repositories
{
    public class OperationTypeRepository : BaseRepository<OperationType>, IOperationTypeRepository
    {
        public OperationTypeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PagedResult<OperationType>> List(int page, int pageSize, OperationTypeSearch search = null)
        {
            var query = DbContext.OperationTypes.AsQueryable();

            search = search ?? new OperationTypeSearch();

            if (!string.IsNullOrWhiteSpace(search.Keyword))
            {
                query = query.Where(o => o.Name.Contains(search.Keyword));
            }

            return await query.OrderByDescending(x => x.Id).GetPagedAsync(page, pageSize);
        }
    }
}