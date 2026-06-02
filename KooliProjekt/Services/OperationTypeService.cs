using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public class OperationTypeService : IOperationTypeService
    {
        private readonly IUnitOfWork _uow;

        public OperationTypeService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PagedResult<OperationType>> List(int page, int pageSize, OperationTypeSearch search = null)
        {
            return await _uow.OperationTypes.List(page, pageSize, search);
        }

        public async Task<OperationType> Get(int id)
        {
            return await _uow.OperationTypes.Get(id);
        }

        public async Task<bool> Save(OperationType operationType)
        {
            _uow.OperationTypes.Add(operationType);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(OperationType operationType)
        {
            _uow.OperationTypes.Update(operationType);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var operationType = await _uow.OperationTypes.Get(id);
            if (operationType == null) return false;
            _uow.OperationTypes.Remove(operationType);
            await _uow.SaveChangesAsync();
            return true;
        }
    }
}
