using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class OperationsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public OperationsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Index_ReturnsSuccessAndCorrectContentType()
        {
            var response = await _client.GetAsync("/Operations");
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdDoesNotExist()
        {
            var response = await _client.GetAsync("/Operations/Details/99999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_Post_InvalidData_ReturnsBadRequestDueToMissingToken()
        {
            var formValues = new Dictionary<string, string> { { "Cost", "-100" } };
            var content = new FormUrlEncodedContent(formValues);

            var response = await _client.PostAsync("/Operations/Create", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}