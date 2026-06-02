using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class InvoicesControllerTests
    {
        private readonly Mock<IInvoiceService> _invoiceServiceMock;
        private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly Mock<IInvoiceNumberService> _invoiceNumberServiceMock;
        private readonly InvoicesController _controller;

        public InvoicesControllerTests()
        {
            _invoiceServiceMock = new Mock<IInvoiceService>();
            _customerServiceMock = new Mock<ICustomerService>();
            _invoiceNumberServiceMock = new Mock<IInvoiceNumberService>();
            _invoiceNumberServiceMock.Setup(x => x.GetNextInvoiceNumber()).Returns("INV-0001");
            _customerServiceMock.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>(), null))
                .ReturnsAsync(new PagedResult<Customer> { Results = new List<Customer>() });
            _controller = new InvoicesController(
                _invoiceServiceMock.Object,
                _customerServiceMock.Object,
                _invoiceNumberServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_view_and_data()
        {
            var page = 1;
            var pagedResult = new PagedResult<Invoice> { Results = new List<Invoice>(), CurrentPage = 1, PageCount = 1, PageSize = 5, RowCount = 0 };
            _invoiceServiceMock.Setup(x => x.List(page, It.IsAny<int>(), null)).ReturnsAsync(pagedResult);
            var result = await _controller.Index(page) as ViewResult;
            Assert.NotNull(result);
            var model = result.Model as InvoicesIndexModel;
            Assert.NotNull(model);
            Assert.Equal(pagedResult, model.Data);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            int? id = null;
            var result = await _controller.Details(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_invoice_is_missing()
        {
            int id = 1;
            _invoiceServiceMock.Setup(x => x.Get(id)).ReturnsAsync((Invoice)null);
            var result = await _controller.Details(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_view_with_model_when_invoice_was_found()
        {
            int id = 1;
            var invoice = new Invoice { Id = id, InvoiceNo = "INV-0001", InvoiceDate = DateTime.Now.AddDays(-1), DueDate = DateTime.Now.AddDays(29) };
            _invoiceServiceMock.Setup(x => x.Get(id)).ReturnsAsync(invoice);
            var result = await _controller.Details(id) as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Details");
            Assert.Equal(invoice, result.Model);
        }

        [Fact]
        public async Task Create_should_return_view()
        {
            var result = await _controller.Create() as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Create");
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_id_is_missing()
        {
            int? id = null;
            var result = await _controller.Edit(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_invoice_is_missing()
        {
            int id = 1;
            _invoiceServiceMock.Setup(x => x.Get(id)).ReturnsAsync((Invoice)null);
            var result = await _controller.Edit(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_view_with_model_when_invoice_was_found()
        {
            int id = 1;
            var invoice = new Invoice { Id = id, InvoiceNo = "INV-0001", InvoiceDate = DateTime.Now.AddDays(-1), DueDate = DateTime.Now.AddDays(29) };
            _invoiceServiceMock.Setup(x => x.Get(id)).ReturnsAsync(invoice);
            var result = await _controller.Edit(id) as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Edit");
            Assert.Equal(invoice, result.Model);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_id_is_missing()
        {
            int? id = null;
            var result = await _controller.Delete(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_invoice_is_missing()
        {
            int id = 1;
            _invoiceServiceMock.Setup(x => x.Get(id)).ReturnsAsync((Invoice)null);
            var result = await _controller.Delete(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_view_with_model_when_invoice_was_found()
        {
            int id = 1;
            var invoice = new Invoice { Id = id, InvoiceNo = "INV-0001", InvoiceDate = DateTime.Now.AddDays(-1), DueDate = DateTime.Now.AddDays(29) };
            _invoiceServiceMock.Setup(x => x.Get(id)).ReturnsAsync(invoice);
            var result = await _controller.Delete(id) as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Delete");
            Assert.Equal(invoice, result.Model);
        }
    }
}