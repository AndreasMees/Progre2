using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public class OperationService : IOperationService
    {
        private readonly IUnitOfWork _uow;

        public OperationService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PagedResult<Operation>> List(int page, int pageSize, OperationSearch search = null)
        {
            return await _uow.Operations.List(page, pageSize, search);
        }

        public async Task<Operation> Get(int id)
        {
            return await _uow.Operations.Get(id);
        }

        public async Task<bool> Save(Operation operation)
        {
            _uow.Operations.Add(operation);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(Operation operation)
        {
            _uow.Operations.Update(operation);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var operation = await _uow.Operations.Get(id);
            if (operation == null) return false;
            _uow.Operations.Remove(operation);
            await _uow.SaveChangesAsync();
            return true;
        }
    }
}
