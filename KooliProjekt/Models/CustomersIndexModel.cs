using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class CustomersIndexModel
    {
        public CustomerSearch Search { get; set; }
        public PagedResult<Customer> Data { get; set; }
    }
}