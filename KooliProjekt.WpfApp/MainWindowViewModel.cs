using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using KooliProjekt.WpfApp.Api;

namespace KooliProjekt.WpfApp
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private readonly IApiClient _apiClient;
        private ObservableCollection<Vehicle> _lists;
        private Vehicle _selectedItem;

        // Õpetaja näitest: nupud ja kood saavad teavitada vigadest läbi selle Actioni
        public Action<string> OnError { get; set; }
        public Func<string, bool> ConfirmDelete { get; set; }

        public ObservableCollection<Vehicle> Lists
        {
            get => _lists;
            set
            {
                _lists = value;
                NotifyPropertyChanged();
            }
        }

        public Vehicle SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand NewCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        // ViewModel võtab konstruktoris vastu liidese kaudu API-kliendi
        public MainWindowViewModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
            Lists = new ObservableCollection<Vehicle>();

            NewCommand = new RelayCommand(_ => ExecuteNew());
            SaveCommand = new RelayCommand(async _ => await ExecuteSave());
            DeleteCommand = new RelayCommand(async _ => await ExecuteDelete());
        }

        public async Task Load()
        {
            // Kutsume välja API-kliendi List() meetodi, mis tagastab Result tüübi
            var result = await _apiClient.List();

            if (result.HasError)
            {
                // Kui tekkis viga, käivitame OnError teavituse
                OnError?.Invoke(result.Error);
                return;
            }

            Lists.Clear();
            if (result.Value != null)
            {
                foreach (var vehicle in result.Value)
                {
                    Lists.Add(vehicle);
                }
            }
        }

        private void ExecuteNew()
        {
            SelectedItem = new Vehicle();
        }

        private async Task ExecuteSave()
        {
            if (SelectedItem == null) return;

            if (string.IsNullOrWhiteSpace(SelectedItem.Manufacturer) ||
                string.IsNullOrWhiteSpace(SelectedItem.Model) ||
                string.IsNullOrWhiteSpace(SelectedItem.LicensePlate))
            {
                OnError?.Invoke("Palun täida kõik väljad enne salvestamist!");
                return;
            }

            var result = await _apiClient.Save(SelectedItem);

            if (result.HasError)
            {
                OnError?.Invoke(result.Error);
                return;
            }

            await Load();
            ExecuteNew();
        }

        private async Task ExecuteDelete()
        {
            if (SelectedItem == null || SelectedItem.Id == 0) return;

            bool canDelete = ConfirmDelete?.Invoke("Kustuta") ?? false;
            if (canDelete)
            {
                var result = await _apiClient.Delete(SelectedItem.Id);

                if (result.HasError)
                {
                    OnError?.Invoke(result.Error);
                    return;
                }

                await Load();
                ExecuteNew();
            }
        }
    }

    // Abiklass Commandide sidumiseks (WPF standard)
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        public RelayCommand(Action<object> execute) => _execute = execute;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _execute(parameter);
        public event EventHandler CanExecuteChanged { add { } remove { } }
    }
}