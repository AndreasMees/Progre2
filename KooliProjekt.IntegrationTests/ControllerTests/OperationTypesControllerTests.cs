using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class OperationTypesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public OperationTypesControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Index_ReturnsSuccessAndCorrectContentType()
        {
            var response = await _client.GetAsync("/OperationTypes");
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task Details_ReturnsSuccess_WhenIdExists()
        {
            var response = await _client.GetAsync("/OperationTypes/Details/1");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdDoesNotExist()
        {
            var response = await _client.GetAsync("/OperationTypes/Details/99999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}