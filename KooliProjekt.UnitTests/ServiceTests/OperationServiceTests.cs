using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class OperationServiceTests
    {
        private ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        private async Task<(Vehicle vehicle, OperationType opType)> AddPrerequisitesAsync(ApplicationDbContext context)
        {
            var vehicle = new Vehicle { Manufacturer = "Volvo", Model = "FH16", LicensePlate = "TST001" };
            var opType = new OperationType { Name = "TestOp" };
            context.Vehicles.Add(vehicle);
            context.OperationTypes.Add(opType);
            await context.SaveChangesAsync();
            return (vehicle, opType);
        }

        [Fact]
        public async Task List_should_return_paged_result()
        {
            using var context = CreateDbContext();
            var (vehicle, opType) = await AddPrerequisitesAsync(context);
            context.Operations.AddRange(
                new Operation { VehicleId = vehicle.Id, OperationTypeId = opType.Id, AssignedEmployeeId = "emp1", Date = DateTime.Today, Status = OperationStatus.Pending },
                new Operation { VehicleId = vehicle.Id, OperationTypeId = opType.Id, AssignedEmployeeId = "emp1", Date = DateTime.Today, Status = OperationStatus.Completed }
            );
            await context.SaveChangesAsync();
            var service = new OperationService(context);

            var result = await service.List(1, 10);

            Assert.NotNull(result);
            Assert.Equal(2, result.RowCount);
        }

        [Fact]
        public async Task Get_should_return_operation_when_found()
        {
            using var context = CreateDbContext();
            var (vehicle, opType) = await AddPrerequisitesAsync(context);
            context.Operations.Add(new Operation { VehicleId = vehicle.Id, OperationTypeId = opType.Id, AssignedEmployeeId = "emp1", Date = DateTime.Today, Status = OperationStatus.Pending });
            await context.SaveChangesAsync();
            var operation = context.Operations.First();
            var service = new OperationService(context);

            var result = await service.Get(operation.Id);

            Assert.NotNull(result);
            Assert.Equal(OperationStatus.Pending, result.Status);
        }

        [Fact]
        public async Task Get_should_return_null_when_not_found()
        {
            using var context = CreateDbContext();
            var service = new OperationService(context);

            var result = await service.Get(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task Save_should_add_new_operation_when_id_is_zero()
        {
            using var context = CreateDbContext();
            var (vehicle, opType) = await AddPrerequisitesAsync(context);
            var service = new OperationService(context);
            var operation = new Operation { Id = 0, VehicleId = vehicle.Id, OperationTypeId = opType.Id, AssignedEmployeeId = "emp1", Date = DateTime.Today, Status = OperationStatus.Pending };

            await service.Save(operation);

            Assert.Equal(1, await context.Operations.CountAsync());
        }

        [Fact]
        public async Task Save_should_update_existing_operation_when_id_is_not_zero()
        {
            using var context = CreateDbContext();
            var (vehicle, opType) = await AddPrerequisitesAsync(context);
            context.Operations.Add(new Operation { VehicleId = vehicle.Id, OperationTypeId = opType.Id, AssignedEmployeeId = "emp1", Date = DateTime.Today, Status = OperationStatus.Pending });
            await context.SaveChangesAsync();
            var operation = context.Operations.First();
            operation.Status = OperationStatus.Completed;
            var service = new OperationService(context);

            await service.Save(operation);

            var updated = await context.Operations.FindAsync(operation.Id);
            Assert.Equal(OperationStatus.Completed, updated!.Status);
        }

        [Fact]
        public async Task Delete_should_remove_operation_when_found()
        {
            using var context = CreateDbContext();
            var (vehicle, opType) = await AddPrerequisitesAsync(context);
            context.Operations.Add(new Operation { VehicleId = vehicle.Id, OperationTypeId = opType.Id, AssignedEmployeeId = "emp1", Date = DateTime.Today, Status = OperationStatus.Pending });
            await context.SaveChangesAsync();
            var operation = context.Operations.First();
            var service = new OperationService(context);

            await service.Delete(operation.Id);

            Assert.Equal(0, await context.Operations.CountAsync());
        }

        [Fact]
        public async Task Delete_should_do_nothing_when_not_found()
        {
            using var context = CreateDbContext();
            var (vehicle, opType) = await AddPrerequisitesAsync(context);
            context.Operations.Add(new Operation { VehicleId = vehicle.Id, OperationTypeId = opType.Id, AssignedEmployeeId = "emp1", Date = DateTime.Today, Status = OperationStatus.Pending });
            await context.SaveChangesAsync();
            var service = new OperationService(context);

            await service.Delete(999);

            Assert.Equal(1, await context.Operations.CountAsync());
        }
    }
}
