using System.Windows;
using KooliProjekt.WpfApp.Api;

namespace KooliProjekt.WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 1. Luuakse API klient
            var apiClient = new ApiClient();

            // 2. Süstitakse see ViewModelile kaasa
            var viewModel = new MainWindowViewModel(apiClient);

            // 3. Seotakse veateate kuvamine õpetaja näite eeskujul
            viewModel.OnError = error =>
            {
                MessageBox.Show(error, "API Tõrge", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            viewModel.ConfirmDelete = _ =>
            {
                var result = MessageBox.Show(
                                "Are you sure you want to delete selected item?",
                                "Delete item",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Stop
                                );
                return (result == MessageBoxResult.Yes);
            };

            DataContext = viewModel;

            // 4. Laaditakse andmed
            await viewModel.Load();
        }
    }
}