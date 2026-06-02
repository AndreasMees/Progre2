using KooliProjekt.Search;

namespace KooliProjekt.Data.Repositories
{
    public interface IInvoiceLineRepository
    {
        Task<PagedResult<InvoiceLine>> List(int page, int pageSize, InvoiceLineSearch search = null);
        Task<InvoiceLine> Get(int id);
        InvoiceLine Add(InvoiceLine invoiceLine);
        InvoiceLine Update(InvoiceLine invoiceLine);
        InvoiceLine Remove(InvoiceLine invoiceLine);
        Task Save(InvoiceLine invoiceLine);
        Task Delete(int id);
    }
}