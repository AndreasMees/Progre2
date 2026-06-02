using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _uow;

        public CustomerService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PagedResult<Customer>> List(int page, int pageSize, CustomerSearch search = null)
        {
            return await _uow.Customers.List(page, pageSize, search);
        }

        public async Task<Customer> Get(int id)
        {
            return await _uow.Customers.Get(id);
        }

        public async Task<bool> Save(Customer customer)
        {
            _uow.Customers.Add(customer);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(Customer customer)
        {
            _uow.Customers.Update(customer);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var customer = await _uow.Customers.Get(id);
            if (customer == null) return false;
            _uow.Customers.Remove(customer);
            await _uow.SaveChangesAsync();
            return true;
        }
    }
}