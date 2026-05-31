using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class InvoicesIndexModel
    {
        public InvoiceSearch Search { get; set; }
        public PagedResult<Invoice> Data { get; set; }
    }
}