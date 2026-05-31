using Microsoft.AspNetCore.Mvc;
using KooliProjekt.Data;
using KooliProjekt.Services;

namespace KooliProjekt.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index(int page = 1)
        {
            return View(await _vehicleService.List(page, 5));
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var vehicle = await _vehicleService.Get(id.Value);
            if (vehicle == null) return NotFound();

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            ModelState.Remove("Id");
            ModelState.Remove("Operations");

            if (ModelState.IsValid)
            {
                await _vehicleService.Save(vehicle);
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var vehicle = await _vehicleService.Get(id.Value);
            if (vehicle == null) return NotFound();

            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehicle vehicle)
        {
            if (id != vehicle.Id) return NotFound();

            ModelState.Remove("Operations");

            if (ModelState.IsValid)
            {
                await _vehicleService.Save(vehicle);
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var vehicle = await _vehicleService.Get(id.Value);
            if (vehicle == null) return NotFound();

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _vehicleService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}