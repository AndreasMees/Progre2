using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;

namespace KooliProjekt.Services
{
    public class OperationTypeService : IOperationTypeService
    {
        private readonly IUnitOfWork _uow;

        public OperationTypeService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PagedResult<OperationType>> List(int page, int pageSize)
        {
            return await _uow.OperationTypeRepository.List(page, pageSize);
        }

        public async Task<OperationType> Get(int id)
        {
            return await _uow.OperationTypeRepository.Get(id);
        }

        public async Task Save(OperationType operationType)
        {
            await _uow.BeginTransaction();
            try
            {
                await _uow.OperationTypeRepository.Save(operationType);
                await _uow.Commit();
            }
            catch
            {
                await _uow.Rollback();
                throw;
            }
        }

        public async Task Delete(int id)
        {
            await _uow.BeginTransaction();
            try
            {
                await _uow.OperationTypeRepository.Delete(id);
                await _uow.Commit();
            }
            catch
            {
                await _uow.Rollback();
                throw;
            }
        }
    }
}