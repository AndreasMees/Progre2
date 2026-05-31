using KooliProjekt.Search;

namespace KooliProjekt.Data.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PagedResult<Customer>> List(int page, int pageSize, CustomerSearch search = null)
        {
            var query = DbContext.Customers.AsQueryable();

            search = search ?? new CustomerSearch();

            if (!string.IsNullOrWhiteSpace(search.Keyword))
            {
                query = query.Where(c => c.Name.Contains(search.Keyword) ||
                                        c.Address.Contains(search.Keyword) ||
                                        c.Phone.Contains(search.Keyword));
            }

            if (!string.IsNullOrWhiteSpace(search.Email))
            {
                query = query.Where(c => c.Email.Contains(search.Email));
            }

            return await query.OrderByDescending(x => x.Id).GetPagedAsync(page, pageSize);
        }
    }
}