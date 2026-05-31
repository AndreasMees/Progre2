using KooliProjekt.Search;

namespace KooliProjekt.Data.Repositories
{
    public interface ICustomerRepository
    {
        Task<PagedResult<Customer>> List(int page, int pageSize, CustomerSearch search = null);
        Task<Customer> Get(int id);
        Task Save(Customer customer);
        Task Delete(int id);
    }
}