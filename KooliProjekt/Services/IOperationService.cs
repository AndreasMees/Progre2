using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IOperationService
    {
        Task<PagedResult<Operation>> List(int page, int pageSize);
        Task<Operation> Get(int id);
        Task Save(Operation operation);
        Task Delete(int id);
    }
}