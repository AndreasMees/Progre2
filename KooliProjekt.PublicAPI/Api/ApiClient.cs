using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace KooliProjekt.PublicAPI
{
    // Autonoomne andmemudel, mis ei sõltu serveri veebipakettidest
    public class Vehicle
    {
        public int Id { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string? LicensePlate { get; set; }
    }

    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri("https://localhost:7136/api/");
            }
        }

        public async Task<Result<List<Vehicle>>> List()
        {
            var result = new Result<List<Vehicle>>();
            try
            {
                // Loeme API vastuse esmalt toore tekstina sisse
                var jsonString = await _httpClient.GetStringAsync("VehiclesApi");

                using (JsonDocument doc = JsonDocument.Parse(jsonString))
                {
                    // Võtame JSON-ist välja ainult "results" massiivi osa
                    if (doc.RootElement.TryGetProperty("results", out JsonElement resultsElement))
                    {
                        result.Value = JsonSerializer.Deserialize<List<Vehicle>>(resultsElement.GetRawText(), new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    }
                    else
                    {
                        result.Error = "Viga: API vastusest puudub 'results' massiiv.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = $"Andmete laadimine ebaõnnestus: {ex.Message}";
            }
            return result;
        }

        public async Task<Result> Save(Vehicle vehicle)
        {
            var result = new Result();
            try
            {
                HttpResponseMessage response;
                if (vehicle.Id == 0)
                {
                    response = await _httpClient.PostAsJsonAsync("VehiclesApi", vehicle);
                }
                else
                {
                    response = await _httpClient.PutAsJsonAsync($"VehiclesApi/{vehicle.Id}", vehicle);
                }

                if (!response.IsSuccessStatusCode)
                {
                    result.Error = $"Salvestamine ebaõnnestus. Serveri staatus: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                result.Error = $"Salvestamisel tekkis viga: {ex.Message}";
            }
            return result;
        }

        public async Task<Result> Delete(int id)
        {
            var result = new Result();
            try
            {
                var response = await _httpClient.DeleteAsync($"VehiclesApi/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    result.Error = $"Kustutamine ebaõnnestus. Serveri staatus: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                result.Error = $"Kustutamisel tekkis viga: {ex.Message}";
            }
            return result;
        }
    }
}