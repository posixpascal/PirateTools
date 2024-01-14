using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using KristofferStrube.Blazor.FileSystem;
using BlazorDownloadFile;
using PirateTools.TravelExpense.WebApp.Services;
using System.Globalization;
using Blazored.Modal;

namespace PirateTools.TravelExpense.WebApp;

public static class Program {
    public static async Task Main(string[] args) {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        builder.Services.AddStorageManagerService();
        builder.Services.AddBlazorDownloadFile();
        builder.Services.AddBlazoredModal();

        builder.Services.AddScoped<FontService>();

        builder.Services.AddScoped<AppDataService>();
        builder.Services.AddSingleton(new CultureInfo("de-DE"));

        await builder.Build().RunAsync();
    }
}