using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace KooliProjekt.PublicAPI
{
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

        // ... (Kõik ülejäänud List(), Save(), Delete() meetodid jäävad täpselt samaks)
        public async Task<Result<List<Vehicle>>> List()
        {
            var result = new Result<List<Vehicle>>();
            try
            {
                var jsonString = await _httpClient.GetStringAsync("VehiclesApi");
                using (JsonDocument doc = JsonDocument.Parse(jsonString))
                {
                    if (doc.RootElement.TryGetProperty("results", out JsonElement resultsElement))
                    {
                        result.Value = JsonSerializer.Deserialize<List<Vehicle>>(resultsElement.GetRawText(), new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
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

                if (!response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    using (JsonDocument doc = JsonDocument.Parse(content))
                    {
                        if (doc.RootElement.TryGetProperty("errors", out JsonElement errorsElement))
                        {
                            result.Errors = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(errorsElement.GetRawText(), new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            }) ?? new Dictionary<string, List<string>>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Result> Delete(int id)
        {
            var result = new Result();
            try
            {
                await _httpClient.DeleteAsync($"VehiclesApi/{id}");
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
    }
}