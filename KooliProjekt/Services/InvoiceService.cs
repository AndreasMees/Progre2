using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _uow;

        public InvoiceService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PagedResult<Invoice>> List(int page, int pageSize, InvoiceSearch search = null)
        {
            return await _uow.Invoices.List(page, pageSize, search);
        }

        public async Task<Invoice> Get(int id)
        {
            return await _uow.Invoices.Get(id);
        }

        public async Task<bool> Save(Invoice invoice)
        {
            _uow.Invoices.Add(invoice);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(Invoice invoice)
        {
            _uow.Invoices.Update(invoice);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var invoice = await _uow.Invoices.Get(id);
            if (invoice == null) return false;
            _uow.Invoices.Remove(invoice);
            await _uow.SaveChangesAsync();
            return true;
        }
    }
}
