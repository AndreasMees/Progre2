using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class VehiclesControllerTests
    {
        private readonly Mock<IVehicleService> _vehicleServiceMock;
        private readonly VehiclesController _controller;

        public VehiclesControllerTests()
        {
            _vehicleServiceMock = new Mock<IVehicleService>();
            _controller = new VehiclesController(_vehicleServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<Vehicle>
            {
                new Vehicle { Id = 1, Manufacturer = "Volvo", Model = "FH16", LicensePlate = "ABC123" },
                new Vehicle { Id = 2, Manufacturer = "Mercedes", Model = "Sprinter", LicensePlate = "DEF456" }
            };
            var pagedResult = new PagedResult<Vehicle> { Results = data };
            _vehicleServiceMock
                .Setup(x => x.List(page, It.IsAny<int>(), null))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as VehiclesIndexModel;
            Assert.NotNull(model);
            Assert.Equal(pagedResult, model.Data);
        }
    }
}