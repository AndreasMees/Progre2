using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface ICustomerService
    {
        Task<PagedResult<Customer>> List(int page, int pageSize, CustomerSearch search = null);
        Task<Customer> Get(int id);
        Task<bool> Save(Customer customer);
        Task<bool> Update(Customer customer);
        Task<bool> Delete(int id);
    }
}