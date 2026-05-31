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
            _controller = new InvoicesController(
                _invoiceServiceMock.Object,
                _customerServiceMock.Object,
                _invoiceNumberServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<Invoice>
            {
                new Invoice { Id = 1, InvoiceNo = "INV-0001", InvoiceDate = DateTime.Now.AddDays(-1), DueDate = DateTime.Now.AddDays(29) },
                new Invoice { Id = 2, InvoiceNo = "INV-0002", InvoiceDate = DateTime.Now.AddDays(-1), DueDate = DateTime.Now.AddDays(29) }
            };
            var pagedResult = new PagedResult<Invoice> { Results = data };
            _invoiceServiceMock
                .Setup(x => x.List(page, It.IsAny<int>(), null))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as InvoicesIndexModel;
            Assert.NotNull(model);
            Assert.Equal(pagedResult, model.Data);
        }
    }
}