using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SalesOrderFront.Interfaces;
using SalesOrderFront.Services;
using SalesOrderFront.ViewModels;
using SalesOrderFront;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/") });
builder.Services.AddScoped<ISalesOrderService, SalesOrderService>();
builder.Services.AddScoped<ISalesOrderViewModel, SalesOrderViewModel>();

await builder.Build().RunAsync();
