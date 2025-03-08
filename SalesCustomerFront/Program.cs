using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SalesCustomerFront.Components;
using SalesCustomerFront.Interfaces;
using SalesCustomerFront.Services;
using SalesCustomerFront.ViewModels;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/") });
builder.Services.AddScoped<ISalesOrderService, SalesOrderService>();
builder.Services.AddScoped<ISalesOrderViewModel, SalesOrderViewModel>();

await builder.Build().RunAsync();