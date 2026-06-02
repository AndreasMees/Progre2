using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IOperationTypeService
    {
        Task<PagedResult<OperationType>> List(int page, int pageSize, OperationTypeSearch search = null);
        Task<OperationType> Get(int id);
        Task<bool> Save(OperationType operationType);
        Task<bool> Update(OperationType operationType);
        Task<bool> Delete(int id);
    }
}
