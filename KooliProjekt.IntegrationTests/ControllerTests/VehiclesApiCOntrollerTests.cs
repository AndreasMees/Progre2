using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class VehiclesApiControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public VehiclesApiControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_ReturnsSuccessAndCorrectContentType()
        {
            var response = await _client.GetAsync("/api/VehiclesApi");
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenIdDoesNotExist()
        {
            var response = await _client.GetAsync("/api/VehiclesApi/99999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Post_ValidData_ReturnsSuccess()
        {
            // Arrange: Kasutame unikaalset numbrimärki (juhuslik number), et andmebaas viga ei viskaks
            var randomPlate = "TEST" + System.Guid.NewGuid().ToString().Substring(0, 4).ToUpper();
            var newVehicle = new 
            {
                Manufacturer = "Audi",
                Model = "A6",
                LicensePlate = randomPlate
            };
            var json = System.Text.Json.JsonSerializer.Serialize(newVehicle);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/VehiclesApi", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}