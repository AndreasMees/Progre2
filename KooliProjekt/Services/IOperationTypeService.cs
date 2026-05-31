using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IOperationTypeService
    {
        Task<PagedResult<OperationType>> List(int page, int pageSize, OperationTypeSearch search = null);
        Task<OperationType> Get(int id);
        Task Save(OperationType operationType);
        Task Delete(int id);
    }
}