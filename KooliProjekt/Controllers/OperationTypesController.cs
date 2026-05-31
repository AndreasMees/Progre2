using Microsoft.AspNetCore.Mvc;
using KooliProjekt.Data;
using KooliProjekt.Services;

namespace KooliProjekt.Controllers
{
    public class OperationTypesController : Controller
    {
        private readonly IOperationTypeService _operationTypeService;

        public OperationTypesController(IOperationTypeService operationTypeService)
        {
            _operationTypeService = operationTypeService;
        }

        // GET: OperationTypes
        public async Task<IActionResult> Index(int page = 1)
        {
            return View(await _operationTypeService.List(page, 5));
        }

        // GET: OperationTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var operationType = await _operationTypeService.Get(id.Value);
            if (operationType == null) return NotFound();

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
                await _operationTypeService.Save(operationType);
                return RedirectToAction(nameof(Index));
            }
            return View(operationType);
        }

        // GET: OperationTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var operationType = await _operationTypeService.Get(id.Value);
            if (operationType == null) return NotFound();

            return View(operationType);
        }

        // POST: OperationTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] OperationType operationType)
        {
            if (id != operationType.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _operationTypeService.Save(operationType);
                return RedirectToAction(nameof(Index));
            }
            return View(operationType);
        }

        // GET: OperationTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var operationType = await _operationTypeService.Get(id.Value);
            if (operationType == null) return NotFound();

            return View(operationType);
        }

        // POST: OperationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _operationTypeService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}