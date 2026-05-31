using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IInvoiceLineService
    {
        Task<PagedResult<InvoiceLine>> List(int page, int pageSize, InvoiceLineSearch search = null);
        Task<InvoiceLine> Get(int id);
        Task Save(InvoiceLine invoiceLine);
        Task Delete(int id);
    }
}