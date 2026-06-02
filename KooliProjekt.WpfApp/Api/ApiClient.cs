using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.WpfApp.Api;

namespace KooliProjekt.WpfApp.Api
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7136/api/");
        }

        public async Task<Result<List<Vehicle>>> List()
        {
            var result = new Result<List<Vehicle>>();

            try
            {
                result.Value = await _httpClient.GetFromJsonAsync<List<Vehicle>>("VehiclesApi");
            }
            catch (Exception ex)
            {
                result.Error = $"Sõidukite laadimine ebaõnnestus: {ex.Message}";
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
                result.Error = $"Andmete salvestamisel tekkis viga: {ex.Message}";
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