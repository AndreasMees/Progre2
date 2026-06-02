using KooliProjekt.Search;

namespace KooliProjekt.Data.Repositories
{
    public interface IOperationRepository
    {
        Task<PagedResult<Operation>> List(int page, int pageSize, OperationSearch search = null);
        Task<Operation> Get(int id);
        Operation Add(Operation operation);
        Operation Update(Operation operation);
        Operation Remove(Operation operation);
        Task Save(Operation operation);
        Task Delete(int id);
    }
}