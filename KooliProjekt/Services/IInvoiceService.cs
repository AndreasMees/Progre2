using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IInvoiceService
    {
        Task<PagedResult<Invoice>> List(int page, int pageSize, InvoiceSearch search = null);
        Task<Invoice> Get(int id);
        Task<bool> Save(Invoice invoice);
        Task<bool> Update(Invoice invoice);
        Task<bool> Delete(int id);
    }
}
