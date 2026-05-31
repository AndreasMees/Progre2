using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Customer>> List(int page, int pageSize)
        {
            return await _context.Customers.GetPagedAsync(page, pageSize);
        }

        public async Task<Customer> Get(int id)
        {
            return await _context.Customers.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(Customer customer)
        {
            if (customer.Id == 0)
                _context.Add(customer);
            else
                _context.Update(customer);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}