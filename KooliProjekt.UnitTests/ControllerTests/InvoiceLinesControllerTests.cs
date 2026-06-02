using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class InvoiceLinesControllerTests
    {
        private readonly Mock<IInvoiceLineService> _invoiceLineServiceMock;
        private readonly Mock<IInvoiceService> _invoiceServiceMock;
        private readonly InvoiceLinesController _controller;

        public InvoiceLinesControllerTests()
        {
            _invoiceLineServiceMock = new Mock<IInvoiceLineService>();
            _invoiceServiceMock = new Mock<IInvoiceService>();
            _invoiceServiceMock.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>(), null))
                .ReturnsAsync(new PagedResult<Invoice> { Results = new List<Invoice>() });
            _controller = new InvoiceLinesController(
                _invoiceLineServiceMock.Object,
                _invoiceServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_view_and_data()
        {
            var page = 1;
            var pagedResult = new PagedResult<InvoiceLine> { Results = new List<InvoiceLine>(), CurrentPage = 1, PageCount = 1, PageSize = 5, RowCount = 0 };
            _invoiceLineServiceMock.Setup(x => x.List(page, It.IsAny<int>(), null)).ReturnsAsync(pagedResult);
            var result = await _controller.Index(page) as ViewResult;
            Assert.NotNull(result);
            var model = result.Model as InvoiceLinesIndexModel;
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
        public async Task Details_should_return_notfound_when_invoiceline_is_missing()
        {
            int id = 1;
            _invoiceLineServiceMock.Setup(x => x.Get(id)).ReturnsAsync((InvoiceLine)null);
            var result = await _controller.Details(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_view_with_model_when_invoiceline_was_found()
        {
            int id = 1;
            var invoiceLine = new InvoiceLine { Id = id, LineItem = "Test", UnitPrice = 50, Quantity = 1, VatRate = 0.2m, Total = 60 };
            _invoiceLineServiceMock.Setup(x => x.Get(id)).ReturnsAsync(invoiceLine);
            var result = await _controller.Details(id) as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Details");
            Assert.Equal(invoiceLine, result.Model);
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
        public async Task Edit_should_return_notfound_when_invoiceline_is_missing()
        {
            int id = 1;
            _invoiceLineServiceMock.Setup(x => x.Get(id)).ReturnsAsync((InvoiceLine)null);
            var result = await _controller.Edit(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_view_with_model_when_invoiceline_was_found()
        {
            int id = 1;
            var invoiceLine = new InvoiceLine { Id = id, LineItem = "Test", UnitPrice = 50, Quantity = 1, VatRate = 0.2m, Total = 60 };
            _invoiceLineServiceMock.Setup(x => x.Get(id)).ReturnsAsync(invoiceLine);
            var result = await _controller.Edit(id) as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Edit");
            Assert.Equal(invoiceLine, result.Model);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_id_is_missing()
        {
            int? id = null;
            var result = await _controller.Delete(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_invoiceline_is_missing()
        {
            int id = 1;
            _invoiceLineServiceMock.Setup(x => x.Get(id)).ReturnsAsync((InvoiceLine)null);
            var result = await _controller.Delete(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_view_with_model_when_invoiceline_was_found()
        {
            int id = 1;
            var invoiceLine = new InvoiceLine { Id = id, LineItem = "Test", UnitPrice = 50, Quantity = 1, VatRate = 0.2m, Total = 60 };
            _invoiceLineServiceMock.Setup(x => x.Get(id)).ReturnsAsync(invoiceLine);
            var result = await _controller.Delete(id) as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Delete");
            Assert.Equal(invoiceLine, result.Model);
        }
    }
}