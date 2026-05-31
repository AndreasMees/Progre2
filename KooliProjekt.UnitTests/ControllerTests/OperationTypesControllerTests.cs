using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class OperationTypesControllerTests
    {
        private readonly Mock<IOperationTypeService> _operationTypeServiceMock;
        private readonly OperationTypesController _controller;

        public OperationTypesControllerTests()
        {
            _operationTypeServiceMock = new Mock<IOperationTypeService>();
            _controller = new OperationTypesController(_operationTypeServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<OperationType>
            {
                new OperationType { Id = 1, Name = "Maintenance" },
                new OperationType { Id = 2, Name = "Repair" }
            };
            var pagedResult = new PagedResult<OperationType> { Results = data };
            _operationTypeServiceMock
                .Setup(x => x.List(page, It.IsAny<int>(), null))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as OperationTypesIndexModel;
            Assert.NotNull(model);
            Assert.Equal(pagedResult, model.Data);
        }
    }
}