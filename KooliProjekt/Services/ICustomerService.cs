using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface ICustomerService
    {
        Task<PagedResult<Customer>> List(int page, int pageSize, CustomerSearch search = null);
        Task<Customer> Get(int id);
        Task Save(Customer customer);
        Task Delete(int id);
    }
}