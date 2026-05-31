namespace KooliProjekt.Data.Repositories
{
    public interface IOperationRepository
    {
        Task<PagedResult<Operation>> List(int page, int pageSize);
        Task<Operation> Get(int id);
        Task Save(Operation operation);
        Task Delete(int id);
    }
}