using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;

namespace KooliProjekt.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _uow;

        public InvoiceService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PagedResult<Invoice>> List(int page, int pageSize)
        {
            return await _uow.InvoiceRepository.List(page, pageSize);
        }

        public async Task<Invoice> Get(int id)
        {
            return await _uow.InvoiceRepository.Get(id);
        }

        public async Task Save(Invoice invoice)
        {
            await _uow.BeginTransaction();
            try
            {
                await _uow.InvoiceRepository.Save(invoice);
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
                await _uow.InvoiceRepository.Delete(id);
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