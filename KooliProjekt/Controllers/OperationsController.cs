using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;

namespace KooliProjekt.Controllers
{
    public class OperationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OperationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Operations
        public async Task<IActionResult> Index(int page = 1)
        {
            var operations = _context.Operations
                .Include(o => o.Vehicle)
                .Include(o => o.OperationType)
                .Include(o => o.AssignedEmployee);
            return View(await operations.GetPagedAsync(page, 5));
        }

        // GET: Operations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operation = await _context.Operations
                .Include(o => o.Vehicle)
                .Include(o => o.OperationType)
                .Include(o => o.AssignedEmployee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (operation == null)
            {
                return NotFound();
            }

            return View(operation);
        }

        // GET: Operations/Create
        public IActionResult Create()
        {
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "LicensePlate");
            ViewData["OperationTypeId"] = new SelectList(_context.OperationTypes, "Id", "Name");
            ViewData["AssignedEmployeeId"] = new SelectList(_context.Users, "Id", "UserName");

            var operation = new Operation();
            operation.Date = DateTime.Now;
            operation.Status = OperationStatus.Pending;

            return View(operation);
        }

        // POST: Operations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VehicleId,OperationTypeId,AssignedEmployeeId,Date,Status,Cost")] Operation operation)
        {
            ModelState.Remove("Vehicle");
            ModelState.Remove("OperationType");
            ModelState.Remove("AssignedEmployee");

            if (ModelState.IsValid)
            {
                _context.Add(operation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "LicensePlate", operation.VehicleId);
            ViewData["OperationTypeId"] = new SelectList(_context.OperationTypes, "Id", "Name", operation.OperationTypeId);
            ViewData["AssignedEmployeeId"] = new SelectList(_context.Users, "Id", "UserName", operation.AssignedEmployeeId);

            return View(operation);
        }

        // GET: Operations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operation = await _context.Operations.FindAsync(id);
            if (operation == null)
            {
                return NotFound();
            }

            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "LicensePlate", operation.VehicleId);
            ViewData["OperationTypeId"] = new SelectList(_context.OperationTypes, "Id", "Name", operation.OperationTypeId);
            ViewData["AssignedEmployeeId"] = new SelectList(_context.Users, "Id", "UserName", operation.AssignedEmployeeId);

            return View(operation);
        }

        // POST: Operations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleId,OperationTypeId,AssignedEmployeeId,Date,Status,Cost")] Operation operation)
        {
            if (id != operation.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Vehicle");
            ModelState.Remove("OperationType");
            ModelState.Remove("AssignedEmployee");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(operation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OperationExists(operation.Id))
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

            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "LicensePlate", operation.VehicleId);
            ViewData["OperationTypeId"] = new SelectList(_context.OperationTypes, "Id", "Name", operation.OperationTypeId);
            ViewData["AssignedEmployeeId"] = new SelectList(_context.Users, "Id", "UserName", operation.AssignedEmployeeId);

            return View(operation);
        }

        // GET: Operations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operation = await _context.Operations
                .Include(o => o.Vehicle)
                .Include(o => o.OperationType)
                .Include(o => o.AssignedEmployee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (operation == null)
            {
                return NotFound();
            }

            return View(operation);
        }

        // POST: Operations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var operation = await _context.Operations.FindAsync(id);
            if (operation != null)
            {
                _context.Operations.Remove(operation);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OperationExists(int id)
        {
            return _context.Operations.Any(e => e.Id == id);
        }
    }
}