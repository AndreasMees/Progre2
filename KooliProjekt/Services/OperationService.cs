using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class OperationService : IOperationService
    {
        private readonly ApplicationDbContext _context;

        public OperationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Operation>> List(int page, int pageSize)
        {
            return await _context.Operations
                .Include(o => o.Vehicle)
                .Include(o => o.OperationType)
                .Include(o => o.AssignedEmployee)
                .GetPagedAsync(page, pageSize);
        }

        public async Task<Operation> Get(int id)
        {
            return await _context.Operations
                .Include(o => o.Vehicle)
                .Include(o => o.OperationType)
                .Include(o => o.AssignedEmployee)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(Operation operation)
        {
            if (operation.Id == 0)
                _context.Add(operation);
            else
                _context.Update(operation);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var operation = await _context.Operations.FindAsync(id);
            if (operation != null)
            {
                _context.Operations.Remove(operation);
                await _context.SaveChangesAsync();
            }
        }
    }
}