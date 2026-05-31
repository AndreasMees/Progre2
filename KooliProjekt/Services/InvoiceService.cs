using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ApplicationDbContext _context;

        public InvoiceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Invoice>> List(int page, int pageSize)
        {
            return await _context.Invoices
                .Include(i => i.Customer)
                .GetPagedAsync(page, pageSize);
        }

        public async Task<Invoice> Get(int id)
        {
            return await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.InvoiceLines)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(Invoice invoice)
        {
            if (invoice.Id == 0)
                _context.Add(invoice);
            else
                _context.Update(invoice);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
            }
        }
    }
}