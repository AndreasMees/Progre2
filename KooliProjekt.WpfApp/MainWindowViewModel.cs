using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KooliProjekt.WpfApp
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "https://localhost:7136/api/VehiclesApi";

        private ObservableCollection<Vehicle> _lists;
        private Vehicle _selectedItem;

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

        // Käsud (Commands) nuppude jaoks
        public ICommand NewCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public MainWindowViewModel()
        {
            _client = new HttpClient();
            Lists = new ObservableCollection<Vehicle>();

            // Seostame nupud funktsioonidega
            NewCommand = new RelayCommand(_ => ExecuteNew());
            SaveCommand = new RelayCommand(async _ => await ExecuteSave());
            DeleteCommand = new RelayCommand(async _ => await ExecuteDelete());
        }

        public async Task Load()
        {
            try
            {
                var response = await _client.GetAsync(_baseUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    using (JsonDocument doc = JsonDocument.Parse(jsonString))
                    {
                        if (doc.RootElement.TryGetProperty("results", out JsonElement resultsElement))
                        {
                            var vehicles = JsonSerializer.Deserialize<List<Vehicle>>(resultsElement.GetRawText(), new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                            Lists.Clear();
                            foreach (var vehicle in vehicles)
                            {
                                Lists.Add(vehicle);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"API viga laadimisel: {ex.Message}");
            }
        }

        // 1. "New" nupu vajutus - loob uue puhta objekti tekstikastide jaoks
        private void ExecuteNew()
        {
            SelectedItem = new Vehicle();
        }

        // 2. "Save" nupu vajutus - salvestab uue (POST) või muudab vana (PUT)
        private async Task ExecuteSave()
        {
            if (SelectedItem == null) return;

            if (string.IsNullOrWhiteSpace(SelectedItem.Manufacturer) ||
                string.IsNullOrWhiteSpace(SelectedItem.Model) ||
                string.IsNullOrWhiteSpace(SelectedItem.LicensePlate))
            {
                System.Windows.MessageBox.Show("Palun täida kõik väljad!");
                return;
            }

            HttpResponseMessage response;

            if (SelectedItem.Id == 0)
            {
                // POST uue lisamiseks
                response = await _client.PostAsJsonAsync(_baseUrl, SelectedItem);
            }
            else
            {
                // PUT muutmiseks
                response = await _client.PutAsJsonAsync($"{_baseUrl}/{SelectedItem.Id}", SelectedItem);
            }

            if (response.IsSuccessStatusCode)
            {
                await Load();
                ExecuteNew(); // Puhastame väljad uue jaoks valmis
                System.Windows.MessageBox.Show("Salvestatud edukalt!");
            }
            else
            {
                System.Windows.MessageBox.Show("Salvestamine ebaõnnestus serveri tõrke tõttu.");
            }
        }

        // 3. "Delete" nupu vajutus - kustutab valitud objekti (DELETE)
        private async Task ExecuteDelete()
        {
            if (SelectedItem == null || SelectedItem.Id == 0) return;

            bool canDelete = ConfirmDelete?.Invoke("Kustuta") ?? false;
            if (canDelete)
            {
                var response = await _client.DeleteAsync($"{_baseUrl}/{SelectedItem.Id}");
                if (response.IsSuccessStatusCode)
                {
                    await Load();
                    ExecuteNew();
                }
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