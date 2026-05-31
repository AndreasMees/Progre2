using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data.Repositories
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PagedResult<Invoice>> List(int page, int pageSize, InvoiceSearch search = null)
        {
            var query = DbContext.Invoices.Include(i => i.Customer).AsQueryable();

            search = search ?? new InvoiceSearch();

            if (!string.IsNullOrWhiteSpace(search.Keyword))
            {
                query = query.Where(i => i.InvoiceNo.Contains(search.Keyword) ||
                                        i.Customer.Name.Contains(search.Keyword));
            }

            if (search.FromDate != null)
            {
                query = query.Where(i => i.InvoiceDate >= search.FromDate);
            }

            if (search.ToDate != null)
            {
                query = query.Where(i => i.InvoiceDate <= search.ToDate);
            }

            return await query.OrderByDescending(x => x.Id).GetPagedAsync(page, pageSize);
        }

        public override async Task<Invoice> Get(int id)
        {
            return await DbContext.Invoices
                .Include(i => i.Customer)
                .Include(i => i.InvoiceLines)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}