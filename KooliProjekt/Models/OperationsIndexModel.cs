using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class OperationsIndexModel
    {
        public OperationSearch Search { get; set; }
        public PagedResult<Operation> Data { get; set; }
    }
}