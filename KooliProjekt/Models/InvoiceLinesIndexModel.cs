using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class InvoiceLinesIndexModel
    {
        public InvoiceLineSearch Search { get; set; }
        public PagedResult<InvoiceLine> Data { get; set; }
    }
}