using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.WinFormsApp;
using KooliProjekt.WinFormsApp.Api;
using Moq;
using Xunit;

namespace KooliProjekt.WinFormsApp.UnitTests
{
    public class Form1PresenterTests
    {
        private readonly Mock<IForm1View> _viewMock;
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Form1Presenter _presenter;

        public Form1PresenterTests()
        {
            // Luuakse liba-objektid (Mockid) liideste põhjal
            _viewMock = new Mock<IForm1View>();
            _apiClientMock = new Mock<IApiClient>();

            // Initsialiseeritakse Presenter testitavasse seisu
            _presenter = new Form1Presenter(_viewMock.Object, _apiClientMock.Object);
        }

        [Fact]
        public async Task LoadData_SovereignCall_WhenApiReturnsError_ShouldShowErrorInView()
        {
            // Arrange
            var apiResult = new Result<List<Vehicle>> { Error = "Ühenduse tõrge" };
            _apiClientMock.Setup(x => x.List()).ReturnsAsync(apiResult);

            // Act
            await _presenter.LoadData();

            // Assert
            // Kontrollitakse, et vaates kutsuti välja ShowError täpselt selle veateatega
            _viewMock.Verify(x => x.ShowError("Ühenduse tõrge"), Times.Once);
            _viewMock.VerifySet(x => x.Vehicles = It.IsAny<List<Vehicle>>(), Times.Never);
        }

        [Fact]
        public async Task LoadData_WhenApiSucceeds_ShouldSetVehiclesInViewAndClearFields()
        {
            // Arrange
            var vehiclesList = new List<Vehicle> { new Vehicle { Id = 1, Manufacturer = "Volvo" } };
            var apiResult = new Result<List<Vehicle>> { Value = vehiclesList };
            _apiClientMock.Setup(x => x.List()).ReturnsAsync(apiResult);

            // Act
            await _presenter.LoadData();

            // Assert
            // Kontrollitakse, et andmed saadeti vaate tabelisse ja väljad tühjendati uue kirje jaoks
            _viewMock.VerifySet(x => x.Vehicles = vehiclesList, Times.Once);
            _viewMock.Verify(x => x.ClearFields(), Times.Once);
        }

        [Fact]
        public async Task SaveVehicle_WhenManufacturerIsEmpty_ShouldShowError()
        {
            // Arrange
            _viewMock.Setup(x => x.VehicleManufacturer).Returns("");

            // Act
            await _presenter.SaveVehicle();

            // Assert
            _viewMock.Verify(x => x.ShowError("Palun täida tootja väli!"), Times.Once);
            _apiClientMock.Verify(x => x.Save(It.IsAny<Vehicle>()), Times.Never);
        }

        [Fact]
        public async Task SaveVehicle_WhenValid_ShouldCallApiSaveAndReloadData()
        {
            // Arrange
            _viewMock.Setup(x => x.VehicleManufacturer).Returns("Audi");
            _apiClientMock.Setup(x => x.Save(It.IsAny<Vehicle>())).ReturnsAsync(new Result());
            _apiClientMock.Setup(x => x.List()).ReturnsAsync(new Result<List<Vehicle>> { Value = new List<Vehicle>() });

            // Act
            await _presenter.SaveVehicle();

            // Assert
            // Kontrollib, et API salvestamist kutsuti korraks ja pärast seda laeti nimekiri uuesti
            _apiClientMock.Verify(x => x.Save(It.Is<Vehicle>(v => v.Manufacturer == "Audi")), Times.Once);
            _apiClientMock.Verify(x => x.List(), Times.Once);
        }

        [Fact]
        public async Task DeleteVehicle_WhenNoVehicleSelected_ShouldShowError()
        {
            // Arrange
            _viewMock.Setup(x => x.SelectedVehicle).Returns((Vehicle)null);
            _presenter.SelectedVehicleChanged(); // Uuendab Presenteri seisu tühjaks

            // Act
            await _presenter.DeleteVehicle();

            // Assert
            _viewMock.Verify(x => x.ShowError("Palun vali tabelist esmalt sõiduk, mida kustutada!"), Times.Once);
            _apiClientMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteVehicle_WhenConfirmed_ShouldCallApiDelete()
        {
            // Arrange
            var selected = new Vehicle { Id = 5, Manufacturer = "BMW" };
            _viewMock.Setup(x => x.SelectedVehicle).Returns(selected);
            _viewMock.Setup(x => x.ConfirmAction(It.IsAny<string>(), It.IsAny<string>())).Returns(true); // Kasutaja vajutab "Yes"

            _apiClientMock.Setup(x => x.Delete(5)).ReturnsAsync(new Result());
            _apiClientMock.Setup(x => x.List()).ReturnsAsync(new Result<List<Vehicle>> { Value = new List<Vehicle>() });

            _presenter.SelectedVehicleChanged(); // Seab Presenterile BMW aktiivseks

            // Act
            await _presenter.DeleteVehicle();

            // Assert
            _apiClientMock.Verify(x => x.Delete(5), Times.Once);
            _apiClientMock.Verify(x => x.List(), Times.Once);
        }
    }
}