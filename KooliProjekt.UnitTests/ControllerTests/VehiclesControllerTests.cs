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
            var page = 1;
            var data = new List<Vehicle>
            {
                new Vehicle { Id = 1, Manufacturer = "Volvo", Model = "FH16", LicensePlate = "ABC123" },
                new Vehicle { Id = 2, Manufacturer = "Mercedes", Model = "Sprinter", LicensePlate = "DEF456" }
            };
            var pagedResult = new PagedResult<Vehicle> { Results = data, CurrentPage = 1, PageCount = 1, PageSize = 5, RowCount = 2 };
            _vehicleServiceMock.Setup(x => x.List(page, It.IsAny<int>(), null)).ReturnsAsync(pagedResult);
            var result = await _controller.Index(page) as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");
            var model = result.Model as VehiclesIndexModel;
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
        public async Task Details_should_return_notfound_when_vehicle_is_missing()
        {
            int id = 1;
            _vehicleServiceMock.Setup(x => x.Get(id)).ReturnsAsync((Vehicle)null);
            var result = await _controller.Details(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_view_with_model_when_vehicle_was_found()
        {
            int id = 1;
            var vehicle = new Vehicle { Id = id, Manufacturer = "Volvo", Model = "FH16", LicensePlate = "ABC123" };
            _vehicleServiceMock.Setup(x => x.Get(id)).ReturnsAsync(vehicle);
            var result = await _controller.Details(id) as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Details");
            Assert.Equal(vehicle, result.Model);
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
        public async Task Edit_should_return_notfound_when_vehicle_is_missing()
        {
            int id = 1;
            _vehicleServiceMock.Setup(x => x.Get(id)).ReturnsAsync((Vehicle)null);
            var result = await _controller.Edit(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_view_with_model_when_vehicle_was_found()
        {
            int id = 1;
            var vehicle = new Vehicle { Id = id, Manufacturer = "Volvo", Model = "FH16", LicensePlate = "ABC123" };
            _vehicleServiceMock.Setup(x => x.Get(id)).ReturnsAsync(vehicle);
            var result = await _controller.Edit(id) as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Edit");
            Assert.Equal(vehicle, result.Model);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_id_is_missing()
        {
            int? id = null;
            var result = await _controller.Delete(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_vehicle_is_missing()
        {
            int id = 1;
            _vehicleServiceMock.Setup(x => x.Get(id)).ReturnsAsync((Vehicle)null);
            var result = await _controller.Delete(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_view_with_model_when_vehicle_was_found()
        {
            int id = 1;
            var vehicle = new Vehicle { Id = id, Manufacturer = "Volvo", Model = "FH16", LicensePlate = "ABC123" };
            _vehicleServiceMock.Setup(x => x.Get(id)).ReturnsAsync(vehicle);
            var result = await _controller.Delete(id) as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Delete");
            Assert.Equal(vehicle, result.Model);
        }

        [Fact]
        public async Task Create_POST_should_return_view_when_modelstate_is_invalid()
        {
            _controller.ModelState.AddModelError("key", "error");
            var vehicle = new Vehicle { Manufacturer = "Volvo", Model = "FH16", LicensePlate = "ABC123" };
            var result = await _controller.Create(vehicle) as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(vehicle, result.Model);
            _vehicleServiceMock.Verify(x => x.Save(It.IsAny<Vehicle>()), Times.Never);
        }

        [Fact]
        public async Task Create_POST_should_redirect_when_modelstate_is_valid()
        {
            var vehicle = new Vehicle { Manufacturer = "Volvo", Model = "FH16", LicensePlate = "ABC123" };
            _vehicleServiceMock.Setup(x => x.Save(vehicle)).Verifiable();
            var result = await _controller.Create(vehicle) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _vehicleServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_POST_should_return_notfound_when_id_mismatch()
        {
            var vehicle = new Vehicle { Id = 2, Manufacturer = "Volvo", Model = "FH16", LicensePlate = "ABC123" };
            var result = await _controller.Edit(1, vehicle) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_POST_should_return_view_when_modelstate_is_invalid()
        {
            _controller.ModelState.AddModelError("key", "error");
            var vehicle = new Vehicle { Id = 1, Manufacturer = "Volvo", Model = "FH16", LicensePlate = "ABC123" };
            var result = await _controller.Edit(1, vehicle) as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(vehicle, result.Model);
            _vehicleServiceMock.Verify(x => x.Save(It.IsAny<Vehicle>()), Times.Never);
        }

        [Fact]
        public async Task Edit_POST_should_redirect_when_modelstate_is_valid()
        {
            var vehicle = new Vehicle { Id = 1, Manufacturer = "Volvo", Model = "FH16", LicensePlate = "ABC123" };
            _vehicleServiceMock.Setup(x => x.Save(vehicle)).Verifiable();
            var result = await _controller.Edit(1, vehicle) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _vehicleServiceMock.VerifyAll();
        }

        [Fact]
        public async Task DeleteConfirmed_should_delete_vehicle()
        {
            int id = 1;
            _vehicleServiceMock.Setup(x => x.Delete(id)).Verifiable();
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _vehicleServiceMock.VerifyAll();
        }
    }
}