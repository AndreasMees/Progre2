using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class InvoiceLineServiceTests
    {
        private ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        private async Task<Invoice> AddInvoiceAsync(ApplicationDbContext context)
        {
            var invoice = new Invoice { InvoiceNo = "INV-0001", InvoiceDate = DateTime.Today };
            context.Invoices.Add(invoice);
            await context.SaveChangesAsync();
            return invoice;
        }

        [Fact]
        public async Task List_should_return_paged_result()
        {
            using var context = CreateDbContext();
            var invoice = await AddInvoiceAsync(context);
            context.InvoiceLines.AddRange(
                new InvoiceLine { LineItem = "Item 1", UnitPrice = 10, Quantity = 2, InvoiceId = invoice.Id },
                new InvoiceLine { LineItem = "Item 2", UnitPrice = 20, Quantity = 1, InvoiceId = invoice.Id }
            );
            await context.SaveChangesAsync();
            var service = new InvoiceLineService(context);

            var result = await service.List(1, 10);

            Assert.NotNull(result);
            Assert.Equal(2, result.RowCount);
        }

        [Fact]
        public async Task Get_should_return_invoiceLine_when_found()
        {
            using var context = CreateDbContext();
            var invoice = await AddInvoiceAsync(context);
            context.InvoiceLines.Add(new InvoiceLine { LineItem = "Item 1", UnitPrice = 10, Quantity = 2, InvoiceId = invoice.Id });
            await context.SaveChangesAsync();
            var line = context.InvoiceLines.First();
            var service = new InvoiceLineService(context);

            var result = await service.Get(line.Id);

            Assert.NotNull(result);
            Assert.Equal("Item 1", result.LineItem);
        }

        [Fact]
        public async Task Get_should_return_null_when_not_found()
        {
            using var context = CreateDbContext();
            var service = new InvoiceLineService(context);

            var result = await service.Get(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task Save_should_add_new_invoiceLine_when_id_is_zero()
        {
            using var context = CreateDbContext();
            var invoice = await AddInvoiceAsync(context);
            var service = new InvoiceLineService(context);
            var line = new InvoiceLine { Id = 0, LineItem = "New Item", UnitPrice = 5, Quantity = 3, InvoiceId = invoice.Id };

            await service.Save(line);

            Assert.Equal(1, await context.InvoiceLines.CountAsync());
        }

        [Fact]
        public async Task Save_should_update_existing_invoiceLine_when_id_is_not_zero()
        {
            using var context = CreateDbContext();
            var invoice = await AddInvoiceAsync(context);
            context.InvoiceLines.Add(new InvoiceLine { LineItem = "Old Item", UnitPrice = 10, Quantity = 1, InvoiceId = invoice.Id });
            await context.SaveChangesAsync();
            var line = context.InvoiceLines.First();
            line.LineItem = "Updated Item";
            var service = new InvoiceLineService(context);

            await service.Save(line);

            var updated = await context.InvoiceLines.FindAsync(line.Id);
            Assert.Equal("Updated Item", updated!.LineItem);
        }

        [Fact]
        public async Task Delete_should_remove_invoiceLine_when_found()
        {
            using var context = CreateDbContext();
            var invoice = await AddInvoiceAsync(context);
            context.InvoiceLines.Add(new InvoiceLine { LineItem = "Item", UnitPrice = 10, Quantity = 1, InvoiceId = invoice.Id });
            await context.SaveChangesAsync();
            var line = context.InvoiceLines.First();
            var service = new InvoiceLineService(context);

            await service.Delete(line.Id);

            Assert.Equal(0, await context.InvoiceLines.CountAsync());
        }

        [Fact]
        public async Task Delete_should_do_nothing_when_not_found()
        {
            using var context = CreateDbContext();
            var invoice = await AddInvoiceAsync(context);
            context.InvoiceLines.Add(new InvoiceLine { LineItem = "Item", UnitPrice = 10, Quantity = 1, InvoiceId = invoice.Id });
            await context.SaveChangesAsync();
            var service = new InvoiceLineService(context);

            await service.Delete(999);

            Assert.Equal(1, await context.InvoiceLines.CountAsync());
        }
    }
}
