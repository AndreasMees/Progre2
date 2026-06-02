using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class VehicleServiceTests
    {
        private ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task List_should_return_paged_result()
        {
            using var context = CreateDbContext();
            context.Vehicles.AddRange(
                new Vehicle { Manufacturer = "Volvo", Model = "FH16", LicensePlate = "ABC001" },
                new Vehicle { Manufacturer = "Mercedes", Model = "Sprinter", LicensePlate = "ABC002" }
            );
            await context.SaveChangesAsync();
            var service = new VehicleService(context);

            var result = await service.List(1, 10);

            Assert.NotNull(result);
            Assert.Equal(2, result.RowCount);
        }

        [Fact]
        public async Task Get_should_return_vehicle_when_found()
        {
            using var context = CreateDbContext();
            context.Vehicles.Add(new Vehicle { Manufacturer = "Volvo", Model = "FH16", LicensePlate = "ABC001" });
            await context.SaveChangesAsync();
            var vehicle = context.Vehicles.First();
            var service = new VehicleService(context);

            var result = await service.Get(vehicle.Id);

            Assert.NotNull(result);
            Assert.Equal("Volvo", result.Manufacturer);
        }

        [Fact]
        public async Task Get_should_return_null_when_not_found()
        {
            using var context = CreateDbContext();
            var service = new VehicleService(context);

            var result = await service.Get(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task Save_should_add_new_vehicle_when_id_is_zero()
        {
            using var context = CreateDbContext();
            var service = new VehicleService(context);
            var vehicle = new Vehicle { Id = 0, Manufacturer = "BMW", Model = "X5", LicensePlate = "NEW001" };

            await service.Save(vehicle);

            Assert.Equal(1, await context.Vehicles.CountAsync());
        }

        [Fact]
        public async Task Save_should_update_existing_vehicle_when_id_is_not_zero()
        {
            using var context = CreateDbContext();
            context.Vehicles.Add(new Vehicle { Manufacturer = "OldMake", Model = "OldModel", LicensePlate = "OLD001" });
            await context.SaveChangesAsync();
            var vehicle = context.Vehicles.First();
            vehicle.Manufacturer = "NewMake";
            var service = new VehicleService(context);

            await service.Save(vehicle);

            var updated = await context.Vehicles.FindAsync(vehicle.Id);
            Assert.Equal("NewMake", updated!.Manufacturer);
        }

        [Fact]
        public async Task Delete_should_remove_vehicle_when_found()
        {
            using var context = CreateDbContext();
            context.Vehicles.Add(new Vehicle { Manufacturer = "Volvo", Model = "FH16", LicensePlate = "DEL001" });
            await context.SaveChangesAsync();
            var vehicle = context.Vehicles.First();
            var service = new VehicleService(context);

            await service.Delete(vehicle.Id);

            Assert.Equal(0, await context.Vehicles.CountAsync());
        }

        [Fact]
        public async Task Delete_should_do_nothing_when_not_found()
        {
            using var context = CreateDbContext();
            context.Vehicles.Add(new Vehicle { Manufacturer = "Volvo", Model = "FH16", LicensePlate = "EXI001" });
            await context.SaveChangesAsync();
            var service = new VehicleService(context);

            await service.Delete(999);

            Assert.Equal(1, await context.Vehicles.CountAsync());
        }
    }
}
