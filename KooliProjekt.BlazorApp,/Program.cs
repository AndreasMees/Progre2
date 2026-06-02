using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using KooliProjekt.PublicAPI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Lisatakse ainult põhikomponent rakenduse käivitamiseks
builder.RootComponents.Add<KooliProjekt.BlazorApp.App>("#app");

// 1. Seadistame HttpClienti baasaadressi Sinu API pordile
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7136/api/") });

// 2. Registreerime ühise teegi kliendi Blazori süsteemi
builder.Services.AddScoped<IApiClient, ApiClient>();

await builder.Build().RunAsync();