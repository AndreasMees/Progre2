using KooliProjekt.Search;

namespace KooliProjekt.Data.Repositories
{
    public interface ICustomerRepository
    {
        Task<PagedResult<Customer>> List(int page, int pageSize, CustomerSearch search = null);
        Task<Customer> Get(int id);
        Customer Add(Customer customer);
        Customer Update(Customer customer);
        Customer Remove(Customer customer);
    }
}