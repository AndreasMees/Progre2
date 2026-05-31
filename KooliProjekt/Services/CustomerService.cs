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
            return await _uow.CustomerRepository.List(page, pageSize, search);
        }

        public async Task<Customer> Get(int id)
        {
            return await _uow.CustomerRepository.Get(id);
        }

        public async Task Save(Customer customer)
        {
            await _uow.BeginTransaction();
            try
            {
                await _uow.CustomerRepository.Save(customer);
                await _uow.Commit();
            }
            catch
            {
                await _uow.Rollback();
                throw;
            }
        }

        public async Task Delete(int id)
        {
            await _uow.BeginTransaction();
            try
            {
                await _uow.CustomerRepository.Delete(id);
                await _uow.Commit();
            }
            catch
            {
                await _uow.Rollback();
                throw;
            }
        }
    }
}