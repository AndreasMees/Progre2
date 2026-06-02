using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class InvoiceServiceTests
    {
        private readonly Mock<IInvoiceRepository> _invoiceRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly InvoiceService _service;

        public InvoiceServiceTests()
        {
            _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.Invoices).Returns(_invoiceRepositoryMock.Object);
            _service = new InvoiceService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task List_should_return_paged_result()
        {
            var page = 1;
            var pageSize = 10;
            var invoices = new List<Invoice>
            {
                new Invoice { Id = 1, InvoiceNo = "INV-001", InvoiceDate = DateTime.Now },
                new Invoice { Id = 2, InvoiceNo = "INV-002", InvoiceDate = DateTime.Now }
            };
            var expectedResult = new PagedResult<Invoice>
            {
                Results = invoices,
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = 2
            };

            _invoiceRepositoryMock.Setup(x => x.List(page, pageSize, null)).ReturnsAsync(expectedResult);

            var result = await _service.List(page, pageSize, null);

            Assert.NotNull(result);
            Assert.Equal(2, result.RowCount);
            _invoiceRepositoryMock.Verify(x => x.List(page, pageSize, null), Times.Once);
        }

        [Fact]
        public async Task Get_should_return_invoice_when_found()
        {
            int id = 1;
            var expectedInvoice = new Invoice { Id = id, InvoiceNo = "INV-001", InvoiceDate = DateTime.Now };

            _invoiceRepositoryMock.Setup(x => x.Get(id)).ReturnsAsync(expectedInvoice);

            var result = await _service.Get(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            _invoiceRepositoryMock.Verify(x => x.Get(id), Times.Once);
        }

        [Fact]
        public async Task Save_should_add_new_invoice()
        {
            var invoice = new Invoice { Id = 0, InvoiceNo = "INV-003", InvoiceDate = DateTime.Now };
            var savedInvoice = new Invoice { Id = 1, InvoiceNo = "INV-003", InvoiceDate = DateTime.Now };

            _invoiceRepositoryMock.Setup(x => x.Add(invoice)).Returns(savedInvoice);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.Save(invoice);

            Assert.True(result);
            _invoiceRepositoryMock.Verify(x => x.Add(invoice), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_should_update_existing_invoice()
        {
            var invoice = new Invoice { Id = 1, InvoiceNo = "INV-001-UPDATED", InvoiceDate = DateTime.Now };

            _invoiceRepositoryMock.Setup(x => x.Update(invoice)).Returns(invoice);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.Update(invoice);

            Assert.True(result);
            _invoiceRepositoryMock.Verify(x => x.Update(invoice), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_should_remove_invoice_when_found()
        {
            int id = 1;
            var invoice = new Invoice { Id = id, InvoiceNo = "INV-001", InvoiceDate = DateTime.Now };

            _invoiceRepositoryMock.Setup(x => x.Get(id)).ReturnsAsync(invoice);
            _invoiceRepositoryMock.Setup(x => x.Remove(invoice)).Returns(invoice);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.Delete(id);

            Assert.True(result);
            _invoiceRepositoryMock.Verify(x => x.Get(id), Times.Once);
            _invoiceRepositoryMock.Verify(x => x.Remove(invoice), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}