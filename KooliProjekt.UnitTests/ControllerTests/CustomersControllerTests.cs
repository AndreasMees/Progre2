using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class CustomersControllerTests
    {
        private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly CustomersController _controller;

        public CustomersControllerTests()
        {
            _customerServiceMock = new Mock<ICustomerService>();
            _controller = new CustomersController(_customerServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<Customer>
            {
                new Customer { Id = 1, Name = "Test Customer 1", Address = "Test St 1", Email = "test1@test.com", Phone = "+37251234567" },
                new Customer { Id = 2, Name = "Test Customer 2", Address = "Test St 2", Email = "test2@test.com", Phone = "+37251234568" }
            };
            var pagedResult = new PagedResult<Customer> { Results = data };
            _customerServiceMock
                .Setup(x => x.List(page, It.IsAny<int>(), null))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as CustomersIndexModel;
            Assert.NotNull(model);
            Assert.Equal(pagedResult, model.Data);
        }
    }
}