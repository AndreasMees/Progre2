using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KooliProjekt.WpfApp
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "https://localhost:7136/api/VehiclesApi";

        public MainWindow()
        {
            InitializeComponent();
            _client = new HttpClient();
            Loaded += async (s, e) => await LoadVehicles();
        }

        private async Task LoadVehicles()
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

                            ListBoxVehicles.ItemsSource = vehicles;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"API-ga ei saanud ühendust: {ex.Message}", "Tõrge");
            }
        }

        private void ListBoxVehicles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxVehicles.SelectedItem is Vehicle selected)
            {
                TxtId.Text = selected.Id.ToString();
                TxtManufacturer.Text = selected.Manufacturer;
                TxtModel.Text = selected.Model;
                TxtLicensePlate.Text = selected.LicensePlate;
                BtnSave.Content = "Salvesta muudatused";
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtManufacturer.Text) ||
                string.IsNullOrWhiteSpace(TxtModel.Text) ||
                string.IsNullOrWhiteSpace(TxtLicensePlate.Text))
            {
                MessageBox.Show("Täida kõik väljad!");
                return;
            }

            var vehicle = new Vehicle
            {
                Manufacturer = TxtManufacturer.Text,
                Model = TxtModel.Text,
                LicensePlate = TxtLicensePlate.Text
            };

            HttpResponseMessage response;

            if (string.IsNullOrEmpty(TxtId.Text))
            {
                response = await _client.PostAsJsonAsync(_baseUrl, vehicle);
            }
            else
            {
                int id = int.Parse(TxtId.Text);
                vehicle.Id = id;
                response = await _client.PutAsJsonAsync($"{_baseUrl}/{id}", vehicle);
            }

            if (response.IsSuccessStatusCode)
            {
                ClearFields();
                await LoadVehicles();
                MessageBox.Show("Salvestatud!");
            }
            else
            {
                MessageBox.Show("Server lükkas salvestamise tagasi.");
            }
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxVehicles.SelectedItem is Vehicle selected)
            {
                var ans = MessageBox.Show($"Kas kustutame auto {selected}?", "Kinnitus", MessageBoxButton.YesNo);
                if (ans == MessageBoxResult.Yes)
                {
                    var response = await _client.DeleteAsync($"{_baseUrl}/{selected.Id}");
                    if (response.IsSuccessStatusCode)
                    {
                        ClearFields();
                        await LoadVehicles();
                    }
                }
            }
        }

        private async void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await LoadVehicles();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            ListBoxVehicles.SelectedItem = null;
            TxtId.Text = string.Empty;
            TxtManufacturer.Text = string.Empty;
            TxtModel.Text = string.Empty;
            TxtLicensePlate.Text = string.Empty;
            BtnSave.Content = "Salvesta uus objekt";
        }
    }
}