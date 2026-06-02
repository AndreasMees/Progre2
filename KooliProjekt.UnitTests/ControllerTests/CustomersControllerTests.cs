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
        public async Task Index_should_return_view_and_data()
        {
            // Arrange
            var page = 1;
            var data = new List<Customer>
            {
                new Customer { Id = 1, Name = "Test 1", Address = "Addr 1", Email = "test1@test.com", Phone = "+37251234567" },
                new Customer { Id = 2, Name = "Test 2", Address = "Addr 2", Email = "test2@test.com", Phone = "+37251234568" }
            };
            var pagedResult = new PagedResult<Customer>
            {
                Results = data,
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 5,
                RowCount = 2
            };
            _customerServiceMock
                .Setup(x => x.List(page, It.IsAny<int>(), null))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");
            var model = result.Model as CustomersIndexModel;
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
        public async Task Details_should_return_notfound_when_customer_is_missing()
        {
            int id = 1;
            _customerServiceMock.Setup(x => x.Get(id)).ReturnsAsync((Customer)null);
            var result = await _controller.Details(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_view_with_model_when_customer_was_found()
        {
            int id = 1;
            var customer = new Customer { Id = id, Name = "Test", Address = "Addr", Email = "t@t.com", Phone = "+37251234567" };
            _customerServiceMock.Setup(x => x.Get(id)).ReturnsAsync(customer);
            var result = await _controller.Details(id) as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Details");
            Assert.Equal(customer, result.Model);
        }

        [Fact]
        public void Create_should_return_view()
        {
            var result = _controller.Create() as ViewResult;
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
        public async Task Edit_should_return_notfound_when_customer_is_missing()
        {
            int id = 1;
            _customerServiceMock.Setup(x => x.Get(id)).ReturnsAsync((Customer)null);
            var result = await _controller.Edit(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_view_with_model_when_customer_was_found()
        {
            int id = 1;
            var customer = new Customer { Id = id, Name = "Test", Address = "Addr", Email = "t@t.com", Phone = "+37251234567" };
            _customerServiceMock.Setup(x => x.Get(id)).ReturnsAsync(customer);
            var result = await _controller.Edit(id) as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Edit");
            Assert.Equal(customer, result.Model);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_id_is_missing()
        {
            int? id = null;
            var result = await _controller.Delete(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_customer_is_missing()
        {
            int id = 1;
            _customerServiceMock.Setup(x => x.Get(id)).ReturnsAsync((Customer)null);
            var result = await _controller.Delete(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_view_with_model_when_customer_was_found()
        {
            int id = 1;
            var customer = new Customer { Id = id, Name = "Test", Address = "Addr", Email = "t@t.com", Phone = "+37251234567" };
            _customerServiceMock.Setup(x => x.Get(id)).ReturnsAsync(customer);
            var result = await _controller.Delete(id) as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Delete");
            Assert.Equal(customer, result.Model);
        }
    }
}