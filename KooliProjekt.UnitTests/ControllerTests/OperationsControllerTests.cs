using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class OperationsControllerTests
    {
        private readonly Mock<IOperationService> _operationServiceMock;
        private readonly Mock<IVehicleService> _vehicleServiceMock;
        private readonly Mock<IOperationTypeService> _operationTypeServiceMock;
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly OperationsController _controller;

        public OperationsControllerTests()
        {
            _operationServiceMock = new Mock<IOperationService>();
            _vehicleServiceMock = new Mock<IVehicleService>();
            _operationTypeServiceMock = new Mock<IOperationTypeService>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            _vehicleServiceMock.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>(), null))
                .ReturnsAsync(new PagedResult<Vehicle> { Results = new List<Vehicle>() });
            _operationTypeServiceMock.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>(), null))
                .ReturnsAsync(new PagedResult<OperationType> { Results = new List<OperationType>() });
            _controller = new OperationsController(
                _operationServiceMock.Object,
                _vehicleServiceMock.Object,
                _operationTypeServiceMock.Object,
                _userManagerMock.Object);
        }

        [Fact]
        public async Task Index_should_return_view_and_data()
        {
            var page = 1;
            var pagedResult = new PagedResult<Operation> { Results = new List<Operation>(), CurrentPage = 1, PageCount = 1, PageSize = 5, RowCount = 0 };
            _operationServiceMock.Setup(x => x.List(page, It.IsAny<int>(), null)).ReturnsAsync(pagedResult);
            var result = await _controller.Index(page) as ViewResult;
            Assert.NotNull(result);
            var model = result.Model as OperationsIndexModel;
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
        public async Task Details_should_return_notfound_when_operation_is_missing()
        {
            int id = 1;
            _operationServiceMock.Setup(x => x.Get(id)).ReturnsAsync((Operation)null);
            var result = await _controller.Details(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_view_with_model_when_operation_was_found()
        {
            int id = 1;
            var operation = new Operation { Id = id, Date = DateTime.Now, Status = OperationStatus.Pending };
            _operationServiceMock.Setup(x => x.Get(id)).ReturnsAsync(operation);
            var result = await _controller.Details(id) as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Details");
            Assert.Equal(operation, result.Model);
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
        public async Task Edit_should_return_notfound_when_operation_is_missing()
        {
            int id = 1;
            _operationServiceMock.Setup(x => x.Get(id)).ReturnsAsync((Operation)null);
            var result = await _controller.Edit(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_view_with_model_when_operation_was_found()
        {
            int id = 1;
            var operation = new Operation { Id = id, Date = DateTime.Now, Status = OperationStatus.Pending };
            _operationServiceMock.Setup(x => x.Get(id)).ReturnsAsync(operation);
            var result = await _controller.Edit(id) as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Edit");
            Assert.Equal(operation, result.Model);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_id_is_missing()
        {
            int? id = null;
            var result = await _controller.Delete(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_operation_is_missing()
        {
            int id = 1;
            _operationServiceMock.Setup(x => x.Get(id)).ReturnsAsync((Operation)null);
            var result = await _controller.Delete(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_view_with_model_when_operation_was_found()
        {
            int id = 1;
            var operation = new Operation { Id = id, Date = DateTime.Now, Status = OperationStatus.Pending };
            _operationServiceMock.Setup(x => x.Get(id)).ReturnsAsync(operation);
            var result = await _controller.Delete(id) as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Delete");
            Assert.Equal(operation, result.Model);
        }
    }
}