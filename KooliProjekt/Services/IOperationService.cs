using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IOperationService
    {
        Task<PagedResult<Operation>> List(int page, int pageSize, OperationSearch search = null);
        Task<Operation> Get(int id);
        Task Save(Operation operation);
        Task Delete(int id);
    }
}