namespace KooliProjekt.Data.Repositories
{
    public interface IOperationTypeRepository
    {
        Task<PagedResult<OperationType>> List(int page, int pageSize);
        Task<OperationType> Get(int id);
        Task Save(OperationType operationType);
        Task Delete(int id);
    }
}