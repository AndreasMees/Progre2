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
        public async Task Index_should_return_view_and_data()
        {
            // Arrange
            var page = 1;
            var data = new List<Vehicle>
            {
                new Vehicle { Id = 1, Manufacturer = "Volvo", Model = "FH16", LicensePlate = "ABC123" },
                new Vehicle { Id = 2, Manufacturer = "Mercedes", Model = "Sprinter", LicensePlate = "DEF456" }
            };
            var pagedResult = new PagedResult<Vehicle>
            {
                Results = data,
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 5,
                RowCount = 2
            };
            _vehicleServiceMock
                .Setup(x => x.List(page, It.IsAny<int>(), null))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");
            var model = result.Model as VehiclesIndexModel;
            Assert.NotNull(model);
            Assert.Equal(pagedResult, model.Data);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Details(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_vehicle_is_missing()
        {
            // Arrange
            int id = 1;
            _vehicleServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Vehicle)null);

            // Act
            var result = await _controller.Details(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_view_with_model_when_vehicle_was_found()
        {
            // Arrange
            int id = 1;
            var vehicle = new Vehicle { Id = id, Manufacturer = "Volvo", Model = "FH16", LicensePlate = "ABC123" };
            _vehicleServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(vehicle);

            // Act
            var result = await _controller.Details(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Details");
            Assert.Equal(vehicle, result.Model);
        }

        [Fact]
        public void Create_should_return_view()
        {
            // Act
            var result = _controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Create");
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Edit(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_vehicle_is_missing()
        {
            // Arrange
            int id = 1;
            _vehicleServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Vehicle)null);

            // Act
            var result = await _controller.Edit(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_view_with_model_when_vehicle_was_found()
        {
            // Arrange
            int id = 1;
            var vehicle = new Vehicle { Id = id, Manufacturer = "Volvo", Model = "FH16", LicensePlate = "ABC123" };
            _vehicleServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(vehicle);

            // Act
            var result = await _controller.Edit(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Edit");
            Assert.Equal(vehicle, result.Model);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_vehicle_is_missing()
        {
            // Arrange
            int id = 1;
            _vehicleServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Vehicle)null);

            // Act
            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_view_with_model_when_vehicle_was_found()
        {
            // Arrange
            int id = 1;
            var vehicle = new Vehicle { Id = id, Manufacturer = "Volvo", Model = "FH16", LicensePlate = "ABC123" };
            _vehicleServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(vehicle);

            // Act
            var result = await _controller.Delete(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Delete");
            Assert.Equal(vehicle, result.Model);
        }
    }
}