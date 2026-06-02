using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using KooliProjekt.WpfApp.Api;

namespace KooliProjekt.WpfApp.UnitTests
{
    public class MainWindowViewModelTests
    {
        private readonly Mock<IApiClient> _apiClientMock;

        public MainWindowViewModelTests()
        {
            _apiClientMock = new Mock<IApiClient>();
        }

        [Fact]
        public void ExecuteNew_ShouldCreateNewVehicleAndClearFields()
        {
            // Arrange
            var viewModel = new MainWindowViewModel(_apiClientMock.Object);
            viewModel.SelectedItem = new Vehicle { Id = 5, Manufacturer = "Audi" };

            // Act
            viewModel.NewCommand.Execute(null);

            // Assert
            Assert.NotNull(viewModel.SelectedItem);
            Assert.Equal(0, viewModel.SelectedItem.Id);
            Assert.Null(viewModel.SelectedItem.Manufacturer);
        }

        [Fact]
        public void SelectedItem_PropertyChange_ShouldNotifyUI()
        {
            // Arrange
            var viewModel = new MainWindowViewModel(_apiClientMock.Object);
            string propertyName = null;

            viewModel.PropertyChanged += (sender, e) =>
            {
                propertyName = e.PropertyName;
            };

            var testVehicle = new Vehicle { Id = 1, Model = "Golf" };

            // Act
            viewModel.SelectedItem = testVehicle;

            // Assert
            Assert.Equal("SelectedItem", propertyName);
        }

        [Fact]
        public async Task ExecuteDelete_WhenCancelled_ShouldNotCallAPI()
        {
            // Arrange
            var viewModel = new MainWindowViewModel(_apiClientMock.Object);
            viewModel.SelectedItem = new Vehicle { Id = 10, Manufacturer = "BMW" };
            viewModel.ConfirmDelete = (msg) => false;

            // Act
            viewModel.DeleteCommand.Execute(null);

            // Assert
            Assert.NotNull(viewModel.SelectedItem);
            Assert.Equal(10, viewModel.SelectedItem.Id);
            _apiClientMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        }
    }
}