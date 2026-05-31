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
            return await _uow.OperationRepository.List(page, pageSize, search);
        }

        public async Task<Operation> Get(int id)
        {
            return await _uow.OperationRepository.Get(id);
        }

        public async Task Save(Operation operation)
        {
            await _uow.BeginTransaction();
            try
            {
                await _uow.OperationRepository.Save(operation);
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
                await _uow.OperationRepository.Delete(id);
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