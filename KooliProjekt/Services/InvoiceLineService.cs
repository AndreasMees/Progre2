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
            return await _uow.InvoiceLineRepository.List(page, pageSize, search);
        }

        public async Task<InvoiceLine> Get(int id)
        {
            return await _uow.InvoiceLineRepository.Get(id);
        }

        public async Task Save(InvoiceLine invoiceLine)
        {
            await _uow.BeginTransaction();
            try
            {
                await _uow.InvoiceLineRepository.Save(invoiceLine);
                await _uow.Commit();
            }
            catch
            {
                await _uow.Rollback();
                throw;
            }
        }

        public async Task Delete(int id)
        {
            await _uow.BeginTransaction();
            try
            {
                await _uow.InvoiceLineRepository.Delete(id);
                await _uow.Commit();
            }
            catch
            {
                await _uow.Rollback();
                throw;
            }
        }
    }
}