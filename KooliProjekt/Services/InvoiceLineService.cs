using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class InvoiceLineService : IInvoiceLineService
    {
        private readonly ApplicationDbContext _context;

        public InvoiceLineService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<InvoiceLine>> List(int page, int pageSize)
        {
            return await _context.InvoiceLines
                .Include(i => i.Invoice)
                .GetPagedAsync(page, pageSize);
        }

        public async Task<InvoiceLine> Get(int id)
        {
            return await _context.InvoiceLines
                .Include(i => i.Invoice)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(InvoiceLine invoiceLine)
        {
            if (invoiceLine.Id == 0)
                _context.Add(invoiceLine);
            else
                _context.Update(invoiceLine);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var invoiceLine = await _context.InvoiceLines.FindAsync(id);
            if (invoiceLine != null)
            {
                _context.InvoiceLines.Remove(invoiceLine);
                await _context.SaveChangesAsync();
            }
        }
    }
}