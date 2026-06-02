using System.Windows;

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
            var viewModel = new MainWindowViewModel();

            // Kustutamise dialoogi sidumine täpselt nagu õpetaja näites
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

            // Laadime andmed API-st
            await viewModel.Load();
        }
    }
}