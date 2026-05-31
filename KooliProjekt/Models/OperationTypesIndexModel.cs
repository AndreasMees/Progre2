using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class OperationTypesIndexModel
    {
        public OperationTypeSearch Search { get; set; }
        public PagedResult<OperationType> Data { get; set; }
    }
}