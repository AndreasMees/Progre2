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
            _controller = new InvoiceLinesController(
                _invoiceLineServiceMock.Object,
                _invoiceServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<InvoiceLine>
            {
                new InvoiceLine { Id = 1, LineItem = "Oil Change", UnitPrice = 50, Quantity = 1, VatRate = 0.2m, Total = 60 },
                new InvoiceLine { Id = 2, LineItem = "Tire Change", UnitPrice = 100, Quantity = 2, VatRate = 0.2m, Total = 240 }
            };
            var pagedResult = new PagedResult<InvoiceLine> { Results = data };
            _invoiceLineServiceMock
                .Setup(x => x.List(page, It.IsAny<int>(), null))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as InvoiceLinesIndexModel;
            Assert.NotNull(model);
            Assert.Equal(pagedResult, model.Data);
        }
    }
}