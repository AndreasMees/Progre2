using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http;

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
            
            // API tagastab JSON andmeid
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task GetById_ReturnsSuccess_WhenIdExists()
        {
            var response = await _client.GetAsync("/api/VehiclesApi/1");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenIdDoesNotExist()
        {
            var response = await _client.GetAsync("/api/VehiclesApi/99999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}