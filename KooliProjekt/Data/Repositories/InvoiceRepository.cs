using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data.Repositories
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<Invoice>> List(int page, int pageSize)
        {
            return await DbContext.Invoices
                .Include(i => i.Customer)
                .OrderByDescending(x => x.Id)
                .GetPagedAsync(page, pageSize);
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