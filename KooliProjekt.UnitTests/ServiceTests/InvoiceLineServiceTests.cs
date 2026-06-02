using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class InvoiceLineServiceTests
    {
        private readonly Mock<IInvoiceLineRepository> _invoiceLineRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly InvoiceLineService _service;

        public InvoiceLineServiceTests()
        {
            _invoiceLineRepositoryMock = new Mock<IInvoiceLineRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.InvoiceLines).Returns(_invoiceLineRepositoryMock.Object);
            _service = new InvoiceLineService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task List_should_return_paged_result()
        {
            var page = 1;
            var pageSize = 10;
            var invoiceLines = new List<InvoiceLine>
            {
                new InvoiceLine { Id = 1, LineItem = "Product 1", UnitPrice = 10, Quantity = 2 },
                new InvoiceLine { Id = 2, LineItem = "Product 2", UnitPrice = 20, Quantity = 1 }
            };
            var expectedResult = new PagedResult<InvoiceLine>
            {
                Results = invoiceLines,
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = 2
            };

            _invoiceLineRepositoryMock.Setup(x => x.List(page, pageSize, null)).ReturnsAsync(expectedResult);

            var result = await _service.List(page, pageSize, null);

            Assert.NotNull(result);
            Assert.Equal(2, result.RowCount);
            _invoiceLineRepositoryMock.Verify(x => x.List(page, pageSize, null), Times.Once);
        }

        [Fact]
        public async Task Get_should_return_invoiceLine_when_found()
        {
            int id = 1;
            var expectedInvoiceLine = new InvoiceLine { Id = id, LineItem = "Product 1", UnitPrice = 10, Quantity = 2 };

            _invoiceLineRepositoryMock.Setup(x => x.Get(id)).ReturnsAsync(expectedInvoiceLine);

            var result = await _service.Get(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            _invoiceLineRepositoryMock.Verify(x => x.Get(id), Times.Once);
        }

        [Fact]
        public async Task Save_should_add_new_invoiceLine()
        {
            var invoiceLine = new InvoiceLine { Id = 0, LineItem = "New Product", UnitPrice = 15, Quantity = 3 };
            var savedInvoiceLine = new InvoiceLine { Id = 1, LineItem = "New Product", UnitPrice = 15, Quantity = 3 };

            _invoiceLineRepositoryMock.Setup(x => x.Add(invoiceLine)).Returns(savedInvoiceLine);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.Save(invoiceLine);

            Assert.True(result);
            _invoiceLineRepositoryMock.Verify(x => x.Add(invoiceLine), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_should_update_existing_invoiceLine()
        {
            var invoiceLine = new InvoiceLine { Id = 1, LineItem = "Updated Product", UnitPrice = 25, Quantity = 2 };

            _invoiceLineRepositoryMock.Setup(x => x.Update(invoiceLine)).Returns(invoiceLine);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.Update(invoiceLine);

            Assert.True(result);
            _invoiceLineRepositoryMock.Verify(x => x.Update(invoiceLine), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_should_remove_invoiceLine_when_found()
        {
            int id = 1;
            var invoiceLine = new InvoiceLine { Id = id, LineItem = "Product 1", UnitPrice = 10, Quantity = 2 };

            _invoiceLineRepositoryMock.Setup(x => x.Get(id)).ReturnsAsync(invoiceLine);
            _invoiceLineRepositoryMock.Setup(x => x.Remove(invoiceLine)).Returns(invoiceLine);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.Delete(id);

            Assert.True(result);
            _invoiceLineRepositoryMock.Verify(x => x.Get(id), Times.Once);
            _invoiceLineRepositoryMock.Verify(x => x.Remove(invoiceLine), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}