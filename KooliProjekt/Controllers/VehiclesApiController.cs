using Microsoft.AspNetCore.Mvc;
using KooliProjekt.Data;      
using KooliProjekt.Services;  
using System.Threading.Tasks;

namespace KooliProjekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesApiController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesApiController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        // GET: api/VehiclesApi
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var pagedResult = await _vehicleService.List(1, 100); 
            return Ok(pagedResult); 
        }

        // GET: api/VehiclesApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var vehicle = await _vehicleService.Get(id); 
            
            if (vehicle == null)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }

        // POST: api/VehiclesApi
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Vehicle vehicle)
        {
            await _vehicleService.Save(vehicle); 
            return Ok(vehicle); 
        }

        // PUT: api/VehiclesApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Vehicle vehicle)
        {
            // 1. Otsime vana objekti andmebaasist
            var existingVehicle = await _vehicleService.Get(id);
            
            if (existingVehicle == null)
            {
                return NotFound(); 
            }

            // 2. Kopeerime uued andmed vana objekti külge (lahendab tracking vea)
            existingVehicle.Manufacturer = vehicle.Manufacturer;
            existingVehicle.Model = vehicle.Model;
            existingVehicle.LicensePlate = vehicle.LicensePlate;
            // Kui sul on Vehicle mudelis veel välju (nt Year vms), lisa need samamoodi siia!

            // 3. Salvestame vana, nüüdseks uuendatud objekti
            await _vehicleService.Save(existingVehicle); 
            
            return NoContent(); 
        }

        // DELETE: api/VehiclesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingVehicle = await _vehicleService.Get(id);
            
            if (existingVehicle == null)
            {
                return NotFound(); 
            }

            await _vehicleService.Delete(id); 
            return NoContent(); 
        }
    }
}