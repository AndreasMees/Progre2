namespace KooliProjekt.Data.Repositories
{
    public class OperationTypeRepository : BaseRepository<OperationType>, IOperationTypeRepository
    {
        public OperationTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}