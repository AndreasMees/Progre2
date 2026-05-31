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
            _controller = new OperationsController(
                _operationServiceMock.Object,
                _vehicleServiceMock.Object,
                _operationTypeServiceMock.Object,
                _userManagerMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<Operation>
            {
                new Operation { Id = 1, Date = DateTime.Now, Status = OperationStatus.Pending },
                new Operation { Id = 2, Date = DateTime.Now, Status = OperationStatus.Completed }
            };
            var pagedResult = new PagedResult<Operation> { Results = data };
            _operationServiceMock
                .Setup(x => x.List(page, It.IsAny<int>(), null))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as OperationsIndexModel;
            Assert.NotNull(model);
            Assert.Equal(pagedResult, model.Data);
        }
    }
}