using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;

namespace KooliProjekt.Controllers
{
    public class OperationTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OperationTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OperationTypes
        public async Task<IActionResult> Index(int page = 1)
        {
            return View(await _context.OperationTypes.GetPagedAsync(page, 5));
        }

        // GET: OperationTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operationType = await _context.OperationTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (operationType == null)
            {
                return NotFound();
            }

            return View(operationType);
        }

        // GET: OperationTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OperationTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] OperationType operationType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(operationType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(operationType);
        }

        // GET: OperationTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operationType = await _context.OperationTypes.FindAsync(id);
            if (operationType == null)
            {
                return NotFound();
            }
            return View(operationType);
        }

        // POST: OperationTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] OperationType operationType)
        {
            if (id != operationType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(operationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OperationTypeExists(operationType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(operationType);
        }

        // GET: OperationTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operationType = await _context.OperationTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (operationType == null)
            {
                return NotFound();
            }

            return View(operationType);
        }

        // POST: OperationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var operationType = await _context.OperationTypes.FindAsync(id);
            if (operationType != null)
            {
                _context.OperationTypes.Remove(operationType);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OperationTypeExists(int id)
        {
            return _context.OperationTypes.Any(e => e.Id == id);
        }
    }
}