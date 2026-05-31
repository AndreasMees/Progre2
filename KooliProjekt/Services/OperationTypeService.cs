using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class OperationTypeService : IOperationTypeService
    {
        private readonly ApplicationDbContext _context;

        public OperationTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<OperationType>> List(int page, int pageSize)
        {
            return await _context.OperationTypes.GetPagedAsync(page, pageSize);
        }

        public async Task<OperationType> Get(int id)
        {
            return await _context.OperationTypes.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(OperationType operationType)
        {
            if (operationType.Id == 0)
                _context.Add(operationType);
            else
                _context.Update(operationType);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var operationType = await _context.OperationTypes.FindAsync(id);
            if (operationType != null)
            {
                _context.OperationTypes.Remove(operationType);
                await _context.SaveChangesAsync();
            }
        }
    }
}