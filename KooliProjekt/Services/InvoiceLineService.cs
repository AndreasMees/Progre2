using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public class InvoiceLineService : IInvoiceLineService
    {
        private readonly IUnitOfWork _uow;

        public InvoiceLineService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PagedResult<InvoiceLine>> List(int page, int pageSize, InvoiceLineSearch search = null)
        {
            return await _uow.InvoiceLines.List(page, pageSize, search);
        }

        public async Task<InvoiceLine> Get(int id)
        {
            return await _uow.InvoiceLines.Get(id);
        }

        public async Task<bool> Save(InvoiceLine invoiceLine)
        {
            _uow.InvoiceLines.Add(invoiceLine);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(InvoiceLine invoiceLine)
        {
            _uow.InvoiceLines.Update(invoiceLine);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var invoiceLine = await _uow.InvoiceLines.Get(id);
            if (invoiceLine == null) return false;
            _uow.InvoiceLines.Remove(invoiceLine);
            await _uow.SaveChangesAsync();
            return true;
        }
    }
}
