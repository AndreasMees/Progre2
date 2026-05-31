namespace KooliProjekt.Data.Repositories
{
    public interface IInvoiceLineRepository
    {
        Task<PagedResult<InvoiceLine>> List(int page, int pageSize);
        Task<InvoiceLine> Get(int id);
        Task Save(InvoiceLine invoiceLine);
        Task Delete(int id);
    }
}