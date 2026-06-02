using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class InvoiceNumberServiceTests
    {
        private ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public void GetNextInvoiceNumber_should_return_INV_0001_when_no_invoices_exist()
        {
            using var context = CreateDbContext();
            var service = new InvoiceNumberService(context);

            var result = service.GetNextInvoiceNumber();

            Assert.Equal("INV-0001", result);
        }

        [Fact]
        public void GetNextInvoiceNumber_should_increment_last_invoice_number()
        {
            using var context = CreateDbContext();
            context.Invoices.Add(new Invoice { InvoiceNo = "INV-0005", InvoiceDate = DateTime.Today });
            context.SaveChanges();
            var service = new InvoiceNumberService(context);

            var result = service.GetNextInvoiceNumber();

            Assert.Equal("INV-0006", result);
        }

        [Fact]
        public void GetNextInvoiceNumber_should_return_INV_0001_when_format_is_invalid()
        {
            using var context = CreateDbContext();
            context.Invoices.Add(new Invoice { InvoiceNo = "BADFORMAT", InvoiceDate = DateTime.Today });
            context.SaveChanges();
            var service = new InvoiceNumberService(context);

            var result = service.GetNextInvoiceNumber();

            Assert.Equal("INV-0001", result);
        }
    }
}
