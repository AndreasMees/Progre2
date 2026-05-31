using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data.Repositories
{
    public class InvoiceLineRepository : BaseRepository<InvoiceLine>, IInvoiceLineRepository
    {
        public InvoiceLineRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<InvoiceLine>> List(int page, int pageSize)
        {
            return await DbContext.InvoiceLines
                .Include(i => i.Invoice)
                .OrderByDescending(x => x.Id)
                .GetPagedAsync(page, pageSize);
        }

        public override async Task<InvoiceLine> Get(int id)
        {
            return await DbContext.InvoiceLines
                .Include(i => i.Invoice)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}