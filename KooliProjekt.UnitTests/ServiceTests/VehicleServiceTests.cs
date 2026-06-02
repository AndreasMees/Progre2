using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class VehicleServiceTests
    {
        private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly VehicleService _service;

        public VehicleServiceTests()
        {
            _vehicleRepositoryMock = new Mock<IVehicleRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.Vehicles).Returns(_vehicleRepositoryMock.Object);
            _service = new VehicleService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task List_should_return_paged_result()
        {
            var page = 1;
            var pageSize = 10;
            var vehicles = new List<Vehicle>
            {
                new Vehicle { Id = 1, Manufacturer = "Volvo", Model = "FH16", LicensePlate = "ABC123" },
                new Vehicle { Id = 2, Manufacturer = "Mercedes", Model = "Sprinter", LicensePlate = "DEF456" }
            };
            var expectedResult = new PagedResult<Vehicle>
            {
                Results = vehicles,
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = 2
            };

            _vehicleRepositoryMock.Setup(x => x.List(page, pageSize, null)).ReturnsAsync(expectedResult);

            var result = await _service.List(page, pageSize, null);

            Assert.NotNull(result);
            Assert.Equal(2, result.RowCount);
            _vehicleRepositoryMock.Verify(x => x.List(page, pageSize, null), Times.Once);
        }

        [Fact]
        public async Task Get_should_return_vehicle_when_found()
        {
            int id = 1;
            var expectedVehicle = new Vehicle { Id = id, Manufacturer = "Volvo", Model = "FH16", LicensePlate = "ABC123" };

            _vehicleRepositoryMock.Setup(x => x.Get(id)).ReturnsAsync(expectedVehicle);

            var result = await _service.Get(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            _vehicleRepositoryMock.Verify(x => x.Get(id), Times.Once);
        }

        [Fact]
        public async Task Save_should_add_new_vehicle()
        {
            var vehicle = new Vehicle { Id = 0, Manufacturer = "BMW", Model = "X5", LicensePlate = "XYZ789" };
            var savedVehicle = new Vehicle { Id = 1, Manufacturer = "BMW", Model = "X5", LicensePlate = "XYZ789" };

            _vehicleRepositoryMock.Setup(x => x.Add(vehicle)).Returns(savedVehicle);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.Save(vehicle);

            Assert.True(result);
            _vehicleRepositoryMock.Verify(x => x.Add(vehicle), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_should_update_existing_vehicle()
        {
            var vehicle = new Vehicle { Id = 1, Manufacturer = "BMW", Model = "X5", LicensePlate = "XYZ789" };

            _vehicleRepositoryMock.Setup(x => x.Update(vehicle)).Returns(vehicle);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.Update(vehicle);

            Assert.True(result);
            _vehicleRepositoryMock.Verify(x => x.Update(vehicle), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_should_remove_vehicle_when_found()
        {
            int id = 1;
            var vehicle = new Vehicle { Id = id, Manufacturer = "Volvo", Model = "FH16", LicensePlate = "ABC123" };

            _vehicleRepositoryMock.Setup(x => x.Get(id)).ReturnsAsync(vehicle);
            _vehicleRepositoryMock.Setup(x => x.Remove(vehicle)).Returns(vehicle);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.Delete(id);

            Assert.True(result);
            _vehicleRepositoryMock.Verify(x => x.Get(id), Times.Once);
            _vehicleRepositoryMock.Verify(x => x.Remove(vehicle), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}