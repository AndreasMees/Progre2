using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace KooliProjekt.IntegrationTests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Lisame seadistuse, mis lülitab AntiForgery tokeni kontrolli testide ajaks välja
                services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
            });
        }
    }
}