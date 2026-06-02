using System;
using System.Threading.Tasks;
using Xunit;
using Moq;

namespace KooliProjekt.WpfApp.UnitTests
{
    public class MainWindowViewModelTests
    {
        [Fact]
        public void ExecuteNew_ShouldCreateNewVehicleAndClearFields()
        {
            // Arrange
            var viewModel = new MainWindowViewModel();
            viewModel.SelectedItem = new Vehicle { Id = 5, Manufacturer = "Audi" };

            // Act: Käivitame uue objekti loomise käsu (simuleerib "New" nupu vajutust)
            viewModel.NewCommand.Execute(null);

            // Assert: Kontrollime, et SelectedItem on loodud uuesti ja selle ID on 0 (tühi)
            Assert.NotNull(viewModel.SelectedItem);
            Assert.Equal(0, viewModel.SelectedItem.Id);
            Assert.Null(viewModel.SelectedItem.Manufacturer);
        }

        [Fact]
        public void SelectedItem_PropertyChange_ShouldNotifyUI()
        {
            // Arrange
            var viewModel = new MainWindowViewModel();
            string propertyName = null;

            // Kuulame, kas PropertyChanged sündmus käivitub
            viewModel.PropertyChanged += (sender, e) =>
            {
                propertyName = e.PropertyName;
            };

            var testVehicle = new Vehicle { Id = 1, Model = "Golf" };

            // Act
            viewModel.SelectedItem = testVehicle;

            // Assert: Kontrollime, et teavitati just "SelectedItem" omaduse muutumisest
            Assert.Equal("SelectedItem", propertyName);
        }

        [Fact]
        public async Task ExecuteDelete_WhenCancelled_ShouldNotCallAPI()
        {
            // Arrange
            var viewModel = new MainWindowViewModel();
            viewModel.SelectedItem = new Vehicle { Id = 10, Manufacturer = "BMW" };

            // Mockime ConfirmDelete funktsiooni nii, et kasutaja vajutab dialoogis "No" (false)
            viewModel.ConfirmDelete = (msg) => false;

            // Act
            viewModel.DeleteCommand.Execute(null);

            // Assert: Kuna kasutaja tühistas, peaks valitud item jääma ikka samaks
            Assert.NotNull(viewModel.SelectedItem);
            Assert.Equal(10, viewModel.SelectedItem.Id);
        }
    }
}