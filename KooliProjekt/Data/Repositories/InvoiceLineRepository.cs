using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data.Repositories
{
    public class InvoiceLineRepository : BaseRepository<InvoiceLine>, IInvoiceLineRepository
    {
        public InvoiceLineRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PagedResult<InvoiceLine>> List(int page, int pageSize, InvoiceLineSearch search = null)
        {
            var query = DbContext.InvoiceLines.Include(i => i.Invoice).AsQueryable();

            search = search ?? new InvoiceLineSearch();

            if (!string.IsNullOrWhiteSpace(search.Keyword))
            {
                query = query.Where(i => i.LineItem.Contains(search.Keyword));
            }

            return await query.OrderByDescending(x => x.Id).GetPagedAsync(page, pageSize);
        }

        public override async Task<InvoiceLine> Get(int id)
        {
            return await DbContext.InvoiceLines
                .Include(i => i.Invoice)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

    }
}