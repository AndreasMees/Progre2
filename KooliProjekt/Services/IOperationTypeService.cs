using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IOperationTypeService
    {
        Task<PagedResult<OperationType>> List(int page, int pageSize);
        Task<OperationType> Get(int id);
        Task Save(OperationType operationType);
        Task Delete(int id);
    }
}