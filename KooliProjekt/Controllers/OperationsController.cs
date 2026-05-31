using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using KooliProjekt.Data;
using KooliProjekt.Services;

namespace KooliProjekt.Controllers
{
    public class OperationsController : Controller
    {
        private readonly IOperationService _operationService;
        private readonly IVehicleService _vehicleService;
        private readonly IOperationTypeService _operationTypeService;
        private readonly UserManager<IdentityUser> _userManager;

        public OperationsController(IOperationService operationService, IVehicleService vehicleService, IOperationTypeService operationTypeService, UserManager<IdentityUser> userManager)
        {
            _operationService = operationService;
            _vehicleService = vehicleService;
            _operationTypeService = operationTypeService;
            _userManager = userManager;
        }

        // GET: Operations
        public async Task<IActionResult> Index(int page = 1)
        {
            return View(await _operationService.List(page, 5));
        }

        // GET: Operations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var operation = await _operationService.Get(id.Value);
            if (operation == null) return NotFound();

            return View(operation);
        }

        // GET: Operations/Create
        public async Task<IActionResult> Create()
        {
            var vehicles = await _vehicleService.List(1, 1000);
            var opTypes = await _operationTypeService.List(1, 1000);
            ViewData["VehicleId"] = new SelectList(vehicles.Results, "Id", "LicensePlate");
            ViewData["OperationTypeId"] = new SelectList(opTypes.Results, "Id", "Name");
            ViewData["AssignedEmployeeId"] = new SelectList(_userManager.Users.ToList(), "Id", "UserName");

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
                await _operationService.Save(operation);
                return RedirectToAction(nameof(Index));
            }

            var vehicles = await _vehicleService.List(1, 1000);
            var opTypes = await _operationTypeService.List(1, 1000);
            ViewData["VehicleId"] = new SelectList(vehicles.Results, "Id", "LicensePlate", operation.VehicleId);
            ViewData["OperationTypeId"] = new SelectList(opTypes.Results, "Id", "Name", operation.OperationTypeId);
            ViewData["AssignedEmployeeId"] = new SelectList(_userManager.Users.ToList(), "Id", "UserName", operation.AssignedEmployeeId);
            return View(operation);
        }

        // GET: Operations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var operation = await _operationService.Get(id.Value);
            if (operation == null) return NotFound();

            var vehicles = await _vehicleService.List(1, 1000);
            var opTypes = await _operationTypeService.List(1, 1000);
            ViewData["VehicleId"] = new SelectList(vehicles.Results, "Id", "LicensePlate", operation.VehicleId);
            ViewData["OperationTypeId"] = new SelectList(opTypes.Results, "Id", "Name", operation.OperationTypeId);
            ViewData["AssignedEmployeeId"] = new SelectList(_userManager.Users.ToList(), "Id", "UserName", operation.AssignedEmployeeId);
            return View(operation);
        }

        // POST: Operations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleId,OperationTypeId,AssignedEmployeeId,Date,Status,Cost")] Operation operation)
        {
            if (id != operation.Id) return NotFound();

            ModelState.Remove("Vehicle");
            ModelState.Remove("OperationType");
            ModelState.Remove("AssignedEmployee");

            if (ModelState.IsValid)
            {
                await _operationService.Save(operation);
                return RedirectToAction(nameof(Index));
            }

            var vehicles = await _vehicleService.List(1, 1000);
            var opTypes = await _operationTypeService.List(1, 1000);
            ViewData["VehicleId"] = new SelectList(vehicles.Results, "Id", "LicensePlate", operation.VehicleId);
            ViewData["OperationTypeId"] = new SelectList(opTypes.Results, "Id", "Name", operation.OperationTypeId);
            ViewData["AssignedEmployeeId"] = new SelectList(_userManager.Users.ToList(), "Id", "UserName", operation.AssignedEmployeeId);
            return View(operation);
        }

        // GET: Operations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var operation = await _operationService.Get(id.Value);
            if (operation == null) return NotFound();

            return View(operation);
        }

        // POST: Operations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _operationService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}