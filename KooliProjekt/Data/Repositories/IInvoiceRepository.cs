using KooliProjekt.Search;

namespace KooliProjekt.Data.Repositories
{
    public interface IInvoiceRepository
    {
        Task<PagedResult<Invoice>> List(int page, int pageSize, InvoiceSearch search = null);
        Task<Invoice> Get(int id);
        Invoice Add(Invoice invoice);
        Invoice Update(Invoice invoice);
        Invoice Remove(Invoice invoice);
        Task Save(Invoice invoice);
        Task Delete(int id);
    }
}