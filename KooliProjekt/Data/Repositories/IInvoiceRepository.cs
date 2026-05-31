namespace KooliProjekt.Data.Repositories
{
    public interface IInvoiceRepository
    {
        Task<PagedResult<Invoice>> List(int page, int pageSize);
        Task<Invoice> Get(int id);
        Task Save(Invoice invoice);
        Task Delete(int id);
    }
}