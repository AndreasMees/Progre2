using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class InvoiceServiceTests
    {
        private ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        private async Task<Customer> AddCustomerAsync(ApplicationDbContext context)
        {
            var customer = new Customer { Name = "TestCust", Address = "Addr", Email = "cust@test.com", Phone = "000" };
            context.Customers.Add(customer);
            await context.SaveChangesAsync();
            return customer;
        }

        [Fact]
        public async Task List_should_return_paged_result()
        {
            using var context = CreateDbContext();
            var customer = await AddCustomerAsync(context);
            context.Invoices.AddRange(
                new Invoice { InvoiceNo = "INV-0001", InvoiceDate = DateTime.Today, CustomerId = customer.Id },
                new Invoice { InvoiceNo = "INV-0002", InvoiceDate = DateTime.Today, CustomerId = customer.Id }
            );
            await context.SaveChangesAsync();
            var service = new InvoiceService(context);

            var result = await service.List(1, 10);

            Assert.NotNull(result);
            Assert.Equal(2, result.RowCount);
        }

        [Fact]
        public async Task Get_should_return_invoice_when_found()
        {
            using var context = CreateDbContext();
            var customer = await AddCustomerAsync(context);
            context.Invoices.Add(new Invoice { InvoiceNo = "INV-0001", InvoiceDate = DateTime.Today, CustomerId = customer.Id });
            await context.SaveChangesAsync();
            var invoice = context.Invoices.First();
            var service = new InvoiceService(context);

            var result = await service.Get(invoice.Id);

            Assert.NotNull(result);
            Assert.Equal("INV-0001", result.InvoiceNo);
        }

        [Fact]
        public async Task Get_should_return_null_when_not_found()
        {
            using var context = CreateDbContext();
            var service = new InvoiceService(context);

            var result = await service.Get(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task Save_should_add_new_invoice_when_id_is_zero()
        {
            using var context = CreateDbContext();
            var service = new InvoiceService(context);
            var invoice = new Invoice { Id = 0, InvoiceNo = "INV-0003", InvoiceDate = DateTime.Today };

            await service.Save(invoice);

            Assert.Equal(1, await context.Invoices.CountAsync());
        }

        [Fact]
        public async Task Save_should_update_existing_invoice_when_id_is_not_zero()
        {
            using var context = CreateDbContext();
            context.Invoices.Add(new Invoice { InvoiceNo = "INV-0001", InvoiceDate = DateTime.Today });
            await context.SaveChangesAsync();
            var invoice = context.Invoices.First();
            invoice.InvoiceNo = "INV-9999";
            var service = new InvoiceService(context);

            await service.Save(invoice);

            var updated = await context.Invoices.FindAsync(invoice.Id);
            Assert.Equal("INV-9999", updated!.InvoiceNo);
        }

        [Fact]
        public async Task Delete_should_remove_invoice_when_found()
        {
            using var context = CreateDbContext();
            context.Invoices.Add(new Invoice { InvoiceNo = "INV-0001", InvoiceDate = DateTime.Today });
            await context.SaveChangesAsync();
            var invoice = context.Invoices.First();
            var service = new InvoiceService(context);

            await service.Delete(invoice.Id);

            Assert.Equal(0, await context.Invoices.CountAsync());
        }

        [Fact]
        public async Task Delete_should_do_nothing_when_not_found()
        {
            using var context = CreateDbContext();
            context.Invoices.Add(new Invoice { InvoiceNo = "INV-0001", InvoiceDate = DateTime.Today });
            await context.SaveChangesAsync();
            var service = new InvoiceService(context);

            await service.Delete(999);

            Assert.Equal(1, await context.Invoices.CountAsync());
        }
    }
}
