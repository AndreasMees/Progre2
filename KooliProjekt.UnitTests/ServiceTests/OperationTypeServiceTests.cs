using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class OperationTypeServiceTests
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
            context.OperationTypes.AddRange(
                new OperationType { Name = "Maintenance" },
                new OperationType { Name = "Repair" }
            );
            await context.SaveChangesAsync();
            var service = new OperationTypeService(context);

            var result = await service.List(1, 10);

            Assert.NotNull(result);
            Assert.Equal(2, result.RowCount);
        }

        [Fact]
        public async Task Get_should_return_operationType_when_found()
        {
            using var context = CreateDbContext();
            context.OperationTypes.Add(new OperationType { Name = "Maintenance" });
            await context.SaveChangesAsync();
            var opType = context.OperationTypes.First();
            var service = new OperationTypeService(context);

            var result = await service.Get(opType.Id);

            Assert.NotNull(result);
            Assert.Equal("Maintenance", result.Name);
        }

        [Fact]
        public async Task Get_should_return_null_when_not_found()
        {
            using var context = CreateDbContext();
            var service = new OperationTypeService(context);

            var result = await service.Get(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task Save_should_add_new_operationType_when_id_is_zero()
        {
            using var context = CreateDbContext();
            var service = new OperationTypeService(context);
            var opType = new OperationType { Id = 0, Name = "Cleaning" };

            await service.Save(opType);

            Assert.Equal(1, await context.OperationTypes.CountAsync());
        }

        [Fact]
        public async Task Save_should_update_existing_operationType_when_id_is_not_zero()
        {
            using var context = CreateDbContext();
            context.OperationTypes.Add(new OperationType { Name = "OldName" });
            await context.SaveChangesAsync();
            var opType = context.OperationTypes.First();
            opType.Name = "NewName";
            var service = new OperationTypeService(context);

            await service.Save(opType);

            var updated = await context.OperationTypes.FindAsync(opType.Id);
            Assert.Equal("NewName", updated!.Name);
        }

        [Fact]
        public async Task Delete_should_remove_operationType_when_found()
        {
            using var context = CreateDbContext();
            context.OperationTypes.Add(new OperationType { Name = "ToDelete" });
            await context.SaveChangesAsync();
            var opType = context.OperationTypes.First();
            var service = new OperationTypeService(context);

            await service.Delete(opType.Id);

            Assert.Equal(0, await context.OperationTypes.CountAsync());
        }

        [Fact]
        public async Task Delete_should_do_nothing_when_not_found()
        {
            using var context = CreateDbContext();
            context.OperationTypes.Add(new OperationType { Name = "Existing" });
            await context.SaveChangesAsync();
            var service = new OperationTypeService(context);

            await service.Delete(999);

            Assert.Equal(1, await context.OperationTypes.CountAsync());
        }
    }
}
