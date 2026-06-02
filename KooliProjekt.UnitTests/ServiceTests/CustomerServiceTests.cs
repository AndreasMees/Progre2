using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class CustomerServiceTests
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
            context.Customers.AddRange(
                new Customer { Name = "Alice", Address = "Street 1", Email = "alice@test.com", Phone = "111" },
                new Customer { Name = "Bob", Address = "Street 2", Email = "bob@test.com", Phone = "222" }
            );
            await context.SaveChangesAsync();
            var service = new CustomerService(context);

            var result = await service.List(1, 10);

            Assert.NotNull(result);
            Assert.Equal(2, result.RowCount);
        }

        [Fact]
        public async Task Get_should_return_customer_when_found()
        {
            using var context = CreateDbContext();
            context.Customers.Add(new Customer { Name = "Alice", Address = "Street 1", Email = "alice@test.com", Phone = "111" });
            await context.SaveChangesAsync();
            var customer = context.Customers.First();
            var service = new CustomerService(context);

            var result = await service.Get(customer.Id);

            Assert.NotNull(result);
            Assert.Equal("Alice", result.Name);
        }

        [Fact]
        public async Task Get_should_return_null_when_not_found()
        {
            using var context = CreateDbContext();
            var service = new CustomerService(context);

            var result = await service.Get(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task Save_should_add_new_customer_when_id_is_zero()
        {
            using var context = CreateDbContext();
            var service = new CustomerService(context);
            var customer = new Customer { Id = 0, Name = "New", Address = "Addr", Email = "new@test.com", Phone = "333" };

            await service.Save(customer);

            Assert.Equal(1, await context.Customers.CountAsync());
        }

        [Fact]
        public async Task Save_should_update_existing_customer_when_id_is_not_zero()
        {
            using var context = CreateDbContext();
            context.Customers.Add(new Customer { Name = "Old", Address = "Addr", Email = "old@test.com", Phone = "444" });
            await context.SaveChangesAsync();
            var customer = context.Customers.First();
            customer.Name = "Updated";
            var service = new CustomerService(context);

            await service.Save(customer);

            var updated = await context.Customers.FindAsync(customer.Id);
            Assert.Equal("Updated", updated!.Name);
        }

        [Fact]
        public async Task Delete_should_remove_customer_when_found()
        {
            using var context = CreateDbContext();
            context.Customers.Add(new Customer { Name = "ToDelete", Address = "Addr", Email = "del@test.com", Phone = "555" });
            await context.SaveChangesAsync();
            var customer = context.Customers.First();
            var service = new CustomerService(context);

            await service.Delete(customer.Id);

            Assert.Equal(0, await context.Customers.CountAsync());
        }

        [Fact]
        public async Task Delete_should_do_nothing_when_not_found()
        {
            using var context = CreateDbContext();
            context.Customers.Add(new Customer { Name = "Existing", Address = "Addr", Email = "ex@test.com", Phone = "666" });
            await context.SaveChangesAsync();
            var service = new CustomerService(context);

            await service.Delete(999);

            Assert.Equal(1, await context.Customers.CountAsync());
        }
    }
}
